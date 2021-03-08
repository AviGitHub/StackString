using System;
using System.Collections.Generic;
using System.Linq;
using WebStack.Data.Models;

namespace WebStack.Data.Services
{
    /// <summary>
    /// EF implementation of the service
    /// TODO: add DB access abstraction layer for CRUD operations so DB instance
    ///       could be injected via dotnet IOC DI mechanism
    ///
    /// NOTE: All exceptions and DB connection issues are being handled in upper layers (by design)
    /// </summary>
    public class DbStackStringService : IStackStringService
    {
        /// <summary> DB instance </summary>
        private readonly StackContext _db = new StackContext();

        private static StackDirection _stackDirection = StackDirection.IN_ORDER;
        private static readonly object _lock = new object();

        public DbStackStringService()
        {
            loadConfiguration();
        }

        /// <summary>
        /// Load stack direction configuration. If not exist - create and save
        /// </summary>
        private void loadConfiguration()
        {
            if (!_db.Configurations.Any())
            {
                _db.Configurations.Add(new StackConfiguration(_stackDirection));
                _db.SaveChanges();
            }
            else
            {
                var loadedConfiguration = _db.Configurations.FirstOrDefault();
                if (loadedConfiguration != null)
                {
                    _stackDirection = loadedConfiguration.StackDirection;
                }
            }
        }

        /// <summary>
        /// Remove the top element of the stack and return its content
        /// </summary>
        /// <returns>If stuck not empty - returns the top element of the stack content
        /// If empty - return null
        /// </returns>
        public string Pop()
        {
            lock (_lock)
            {
                string ret = null;

                if (!_db.StackStrings.Any())
                {
                    return null;
                }

                // Find the top element according to the stack order, save its value for return and remove it
                if (_stackDirection == StackDirection.IN_ORDER)
                {
                    var left = _db.StackStrings
                        .SingleOrDefault(s => s.LeftId == StackString.LEFT_ID);

                    if (left != null)
                    {
                        ret = left.Content;
                        var newLeft = _db.StackStrings
                            .SingleOrDefault(s => s.LeftId == left.StackStringId);

                        if (newLeft != null)
                        {
                            newLeft.LeftId = StackString.LEFT_ID;
                        }

                        _db.StackStrings.Remove(left);
                        _db.SaveChanges();
                    }
                }
                else
                {
                    var right = _db.StackStrings
                        .SingleOrDefault(s => s.RightId == StackString.RIGHT_ID);

                    if (right != null)
                    {
                        ret = right.Content;
                        var newRight = _db.StackStrings
                            .SingleOrDefault(s => s.RightId == right.StackStringId);

                        if (newRight != null)
                        {
                            newRight.RightId = StackString.RIGHT_ID;
                        }

                        _db.StackStrings.Remove(right);
                        _db.SaveChanges();
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// Return the content of the top element in the stack
        /// </summary>
        /// <returns>The content of the top element in the stack.
        /// If stack is empty - return null
        /// </returns>
        public string Pick()
        {
            lock (_lock)
            {
                if (!_db.StackStrings.Any())
                {
                    return null;
                }

                // Find the top element according to the stack order, and return it
                if (_stackDirection == StackDirection.IN_ORDER)
                {
                    var left = _db.StackStrings
                        .SingleOrDefault(s => s.LeftId == StackString.LEFT_ID);

                    if (left != null)
                    {
                        return left.Content;
                    }
                }
                else
                {
                    var right = _db.StackStrings
                        .SingleOrDefault(s => s.RightId == StackString.RIGHT_ID);

                    if (right != null)
                    {
                        return right.Content;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Revert the order of the stack (bottom element is now top element)
        /// </summary>
        /// <returns>true for success
        /// If stack is empty - return false
        /// </returns>
        public bool Revert()
        {
            lock (_lock)
            {
                if (!_db.StackStrings.Any())
                {
                    return false;
                }

                _stackDirection = _stackDirection switch
                {
                    StackDirection.IN_ORDER => StackDirection.REVERTED,
                    StackDirection.REVERTED => StackDirection.IN_ORDER,
                    _ => throw new ArgumentOutOfRangeException()
                };

                var loadedConfiguration = _db.Configurations.FirstOrDefault();
                if (loadedConfiguration != null)
                {
                    loadedConfiguration.StackDirection = _stackDirection;

                    _db.SaveChanges();
                }

                return true;
            }
        }

        /// <summary>
        /// Push element to the top of the stack
        /// </summary>
        /// <param name="newString">content</param>
        public void Push(string newString)
        {
            lock (_lock)
            {
                // First create new StackString object and save it to DB to get a new ID
                var stackString = new StackString(newString);
                var record = _db.StackStrings.Add(stackString);

                _db.SaveChanges();

                // Connect it via 2way binding to the top element of the stack according to the order
                if (_stackDirection == StackDirection.IN_ORDER)
                {
                    var left = _db.StackStrings
                        .SingleOrDefault(s => s.LeftId == StackString.LEFT_ID);

                    if (left != null)
                    {
                        left.LeftId = stackString.StackStringId;
                        stackString.RightId = left.StackStringId;
                        stackString.LeftId = StackString.LEFT_ID;
                    }
                }
                else
                {
                    var right = _db.StackStrings
                        .SingleOrDefault(s => s.RightId == StackString.RIGHT_ID);

                    if (right != null)
                    {
                        right.RightId = stackString.StackStringId;
                        stackString.LeftId = right.StackStringId;
                        stackString.RightId = StackString.RIGHT_ID;
                    }
                }

                // If one of the sides is not connected - this is the first element
                if (stackString.LeftId == StackString.INVALID_ID ||
                    stackString.RightId == StackString.INVALID_ID)
                {
                    stackString.LeftId = StackString.LEFT_ID;
                    stackString.RightId = StackString.RIGHT_ID;
                }

                _db.SaveChanges();
            }
        }
    }
}