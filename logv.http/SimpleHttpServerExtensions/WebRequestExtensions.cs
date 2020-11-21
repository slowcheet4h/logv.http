﻿﻿/*
     Copyright 2012 Kolja Dummann <k.dummann@gmail.com>

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */

using System.Net;
using System.Text;

namespace logv.http
{
    /// <summary>
    /// Helper Class with extension Methods for WebRequest
    /// </summary>
    public static class WebRequestExtensions
    {
        /// <summary>
        /// Writes the specified req.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static WebRequest Write(this WebRequest req, string data)
        {
            var bytez = req.GetEncoding().GetBytes(data);
            req.GetRequestStream().Write(bytez, 0, bytez.Length);            
            return req;
        }

        /// <summary>
        /// Gets the encoding.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        public static Encoding GetEncoding(this WebRequest req)
        {
            //todo read content-encoding an return the correct encoding
            return Encoding.UTF8;
        }
    }
}
