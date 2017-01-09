// MIT License
// 
// Copyright (c) 2016 Wojciech Nag�rski
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

using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using ExtendedXmlSerialization.Conversion.Write;
using ExtendedXmlSerialization.Core;
using ExtendedXmlSerialization.Core.Specifications;

namespace ExtendedXmlSerialization.Conversion.Legacy
{
    class TypeEmittingWriter : DecoratedWriter
    {
        readonly private static ISpecification<TypeInfo> Specification =
            /*new InverseSpecification<TypeInfo>(TypeEqualitySpecification<string>.Default)*/
            AlwaysSpecification<TypeInfo>.Default;

        private readonly ISpecification<TypeInfo> _specification;

        public TypeEmittingWriter(IWriter writer) : this(Specification, writer) {}

        public TypeEmittingWriter(ISpecification<TypeInfo> specification, IWriter writer) : base(writer)
        {
            _specification = specification;
        }

        public override void Write(XmlWriter writer, object instance)
        {
            var type = new Typed(instance.GetType());
            var tracker = Tracker.Default.Get(writer);
            if (_specification.IsSatisfiedBy(type) && tracker.Add(instance))
            {
                writer.WriteAttributeString(ExtendedXmlSerializer.Type,
                                            LegacyTypeFormatter.Default.Format(type));
            }

            base.Write(writer, instance);
        }

        sealed class Tracker : WeakCache<XmlWriter, HashSet<object>>
        {
            public static Tracker Default { get; } = new Tracker();
            Tracker() : base(_ => new HashSet<object>()) {}
        }
    }
}