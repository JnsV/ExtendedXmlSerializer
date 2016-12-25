﻿// MIT License
// 
// Copyright (c) 2016 Wojciech Nagórski
//                    Michael DeMond
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using ExtendedXmlSerialization.Core.Sources;
using ExtendedXmlSerialization.Model.Write;

namespace ExtendedXmlSerialization.Configuration.Write
{
    public interface IEncryptionFactory : IParameterizedSource<IElement, IPropertyEncryption> {}

    class EncryptionFactory : IEncryptionFactory
    {
        private readonly ISerializationToolsFactory _factory;

        public EncryptionFactory(ISerializationToolsFactory factory)
        {
            _factory = factory;
        }

        public IPropertyEncryption Get(IElement parameter)
        {
            var member = parameter as IMember;
            if (member != null)
            {
                var configuration = _factory.GetConfiguration(member.DeclaringType);
                if (configuration != null)
                {
                    var allow =
                        configuration.CheckPropertyEncryption(member.Name);
                    var result = allow ? _factory.EncryptionAlgorithm : null;
                    return result;
                }
            }
            return null;
        }
    }
}