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

using System.Reflection;
using ExtendedXmlSerialization.ContentModel.Converters;
using ExtendedXmlSerialization.ContentModel.Members;
using ExtendedXmlSerialization.Core.Specifications;

namespace ExtendedXmlSerialization.ExtensionModel
{
	class RuntimeMemberSpecifications : IRuntimeMemberSpecifications
	{
		readonly static TypeInfo Type = typeof(string).GetTypeInfo();
		readonly static RuntimeMemberSpecification Always = new RuntimeMemberSpecification(AlwaysSpecification<object>.Default);

		readonly IConverterSource _source;
		readonly IRuntimeMemberSpecification _text;
		readonly IRuntimeMemberSpecifications _specifications;

		public RuntimeMemberSpecifications(IRuntimeMemberSpecification text, IRuntimeMemberSpecifications specifications,
		                                   IConverterSource source)
		{
			_source = source;
			_text = text;
			_specifications = specifications;
		}

		public IRuntimeMemberSpecification Get(MemberInfo parameter) => _specifications.Get(parameter) ?? From(parameter);

		IRuntimeMemberSpecification From(MemberDescriptor descriptor)
		{
			var supported = _source.IsSatisfiedBy(descriptor.MemberType);
			var result = supported ? Equals(descriptor.MemberType, Type) ? _text : Always : null;
			return result;
		}
	}
}