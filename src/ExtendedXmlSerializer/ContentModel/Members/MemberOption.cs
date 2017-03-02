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

using System;
using ExtendedXmlSerialization.TypeModel;

namespace ExtendedXmlSerialization.ContentModel.Members
{
	class MemberOption : MemberOptionBase
	{
		readonly static SetterFactory SetterFactory = SetterFactory.Default;

		public static MemberOption Default { get; } = new MemberOption();
		MemberOption() : this(AssignableMemberSpecification.Default) {}

		readonly ISetterFactory _setter;

		public MemberOption(IMemberSpecification specification) : this(specification, SetterFactory) {}

		public MemberOption(IMemberSpecification specification, ISetterFactory setter) : base(specification)
		{
			_setter = setter;
		}

		protected override IMember Create(MemberProfile profile, Func<object, object> getter)
			=> CreateMember(profile, getter, _setter.Get(profile.Metadata));

		protected virtual IMember CreateMember(MemberProfile profile, Func<object, object> getter,
		                                       Action<object, object> setter)
		{
			var adapter = new MemberAdapterSelector(getter, setter).Get(profile);
			var result = CreateMember(profile, adapter);
			return result;
		}

		protected virtual IMember CreateMember(MemberProfile profile, IMemberAdapter adapter)
			=> new Member(profile.Specification, adapter, profile.Reader, profile.Writer);
	}
}