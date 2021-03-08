using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStack.Data.Models;

namespace WebStack.Data.Services
{
    public interface IStackStringService
    {
        /// <summary>
        /// Return top element content and remove it from the stack
        /// </summary>
        /// <returns>Top element string content of null in case the stack is empty</returns>
        string Pop();

        /// <summary>
        /// Return top element content
        /// </summary>
        /// <returns>Top element string content of null in case the stack is empty</returns>
        string Pick();

        /// <summary>
        /// Revert the order of the stack (bottom element is now top element)
        /// </summary>
        /// <returns>true for success
        /// If stack is empty - return false
        /// </returns>
        bool Revert();

        /// <summary>
        /// Push element to the top of the stack
        /// </summary>
        /// <param name="newString">content</param>
        void Push(string newString);
    }
}