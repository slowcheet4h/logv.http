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

using System;
using System.IO;

namespace logv.http
{
    /// <summary>
    /// 
    /// </summary>
    public class AsynStreamCopyCompleteEventArgs : EventArgs
    {
        readonly Stream inputStream;
        readonly Stream outputStream;

        /// <summary>
        /// Gets the input stream.
        /// </summary>
        /// <value>
        /// The input stream.
        /// </value>
        public Stream InputStream { get { return inputStream; } }
        /// <summary>
        /// Gets the output stream.
        /// </summary>
        /// <value>
        /// The output stream.
        /// </value>
        public Stream OutputStream { get { return outputStream; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsynStreamCopyCompleteEventArgs" /> class.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        public AsynStreamCopyCompleteEventArgs(Stream input, Stream output)
        {
            inputStream = input;
            outputStream = output;
        }
    }
    /// <summary>
    /// Copies data from one stream into another using the async pattern. Copies are done in 4k blocks.
    /// </summary>
    public class AsyncStreamCopier
    {
        readonly Stream _input;
        readonly Stream _output;

        byte[] buffer = new byte[65536];

        /// <summary>
        /// Raised when all data is copied
        /// </summary>
        public event EventHandler<AsynStreamCopyCompleteEventArgs> Completed;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncStreamCopier" /> class.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        public AsyncStreamCopier(Stream input, Stream output)
        {
            _input = input;
            _output = output;
        }

        /// <summary>
        /// Starts the copying
        /// </summary>
        public void Copy()
        {
            GetData();
        }

        void GetData()
        {
            _input.BeginRead(buffer, 0, buffer.Length, ReadComplete, null);
        }

        void ReadComplete(IAsyncResult result)
        {

            int bytes = _input.EndRead(result);

            if (bytes == 0)
            {
                RaiseComplete();
                return;
            }

            try
            {
                _output.Write(buffer, 0, bytes);

                GetData();
            }
            catch (Exception)
            {
               //we swallow all since there is nothing we can do now
                RaiseComplete();
            }
        }

        void RaiseComplete()
        {
            var handler = Completed;

            if (handler != null)
                handler(this,new AsynStreamCopyCompleteEventArgs(_input, _output));
        }
    }
}
