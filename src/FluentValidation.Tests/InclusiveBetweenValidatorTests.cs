#region License
// Copyright (c) .NET Foundation and contributors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// The latest version of this file can be found at https://github.com/FluentValidation/FluentValidation
#endregion

namespace FluentValidation.Tests;

using System;
using System.Linq;
using Xunit;
using Validators;

public class InclusiveBetweenValidatorTests {
	public InclusiveBetweenValidatorTests() {
		CultureScope.SetDefaultCulture();
	}

	[Fact]
	public void When_the_value_is_between_the_range_specified_then_the_validator_should_pass() {
		var validator = new TestValidator(v => v.RuleFor(x => x.Id).InclusiveBetween(1, 10));
		var result = validator.Validate(new Person { Id = 5 });
		result.IsValid.ShouldBeTrue();
	}

	[Fact]
	public void When_the_value_is_smaller_than_the_range_then_the_validator_should_fail() {
		var validator = new TestValidator(v => v.RuleFor(x => x.Id).InclusiveBetween(1, 10));
		var result = validator.Validate(new Person { Id = 0 });
		result.IsValid.ShouldBeFalse();
	}

	[Fact]
	public void When_the_text_is_larger_than_the_range_then_the_validator_should_fail() {
		var validator = new TestValidator(v => v.RuleFor(x => x.Id).InclusiveBetween(1, 10));
		var result = validator.Validate(new Person() { Id = 11 });
		result.IsValid.ShouldBeFalse();
	}

	[Fact]
	public void When_the_value_is_exactly_the_size_of_the_upper_bound_then_the_validator_should_pass() {
		var validator = new TestValidator(v => v.RuleFor(x => x.Id).InclusiveBetween(1, 10));
		var result = validator.Validate(new Person { Id = 10 });
		result.IsValid.ShouldBeTrue();
	}

	[Fact]
	public void When_the_value_is_exactly_the_size_of_the_lower_bound_then_the_validator_should_pass() {
		var validator = new TestValidator(v => v.RuleFor(x => x.Id).InclusiveBetween(1, 10));
		var result = validator.Validate(new Person { Id = 1 });
		result.IsValid.ShouldBeTrue();
	}

	[Fact]
	public void When_the_to_is_smaller_than_the_from_then_the_validator_should_throw() {
		Assert.Throws<ArgumentOutOfRangeException>(() => new TestValidator(v => v.RuleFor(x => x.Id).InclusiveBetween(10, 1)));
	}

	[Fact]
	public void When_the_validator_fails_the_error_message_should_be_set() {
		var validator = new TestValidator(v => v.RuleFor(x => x.Id).InclusiveBetween(1, 10));
		var result = validator.Validate(new Person { Id = 0 });
		result.Errors.Single().ErrorMessage.ShouldEqual("'Id' must be between 1 and 10. You entered 0.");
	}

	[Fact]
	public void To_and_from_properties_should_be_set() {
		var validator = RangeValidatorFactory.CreateInclusiveBetween<Person ,int>(1, 10);
		validator.From.ShouldEqual(1);
		validator.To.ShouldEqual(10);
	}

	[Fact]
	public void When_the_value_is_between_the_range_specified_then_the_validator_should_pass_for_strings() {
		var validator = new TestValidator(v => v.RuleFor(x => x.Surname).InclusiveBetween("aa", "zz"));
		var result = validator.Validate(new Person { Surname = "bbb" });
		result.IsValid.ShouldBeTrue();
	}

	[Fact]
	public void When_the_value_is_smaller_than_the_range_then_the_validator_should_fail_for_strings() {
		var validator = new TestValidator(v => v.RuleFor(x => x.Surname).InclusiveBetween("bbb", "zz"));
		var result = validator.Validate(new Person { Surname = "aaa" });
		result.IsValid.ShouldBeFalse();
	}

	[Fact]
	public void When_the_text_is_larger_than_the_range_then_the_validator_should_fail_for_strings() {
		var validator = new TestValidator(v => v.RuleFor(x => x.Surname).InclusiveBetween("aaa", "bbb"));
		var result = validator.Validate(new Person { Surname = "zzz" });
		result.IsValid.ShouldBeFalse();
	}

	[Fact]
	public void When_the_value_is_exactly_the_size_of_the_upper_bound_then_the_validator_should_pass_for_strings() {
		var validator = new TestValidator(v => v.RuleFor(x => x.Surname).InclusiveBetween("aa", "zz"));
		var result = validator.Validate(new Person { Surname = "aa" });
		result.IsValid.ShouldBeTrue();
	}

	[Fact]
	public void When_the_value_is_exactly_the_size_of_the_lower_bound_then_the_validator_should_pass_for_strings() {
		var validator = new TestValidator(v => v.RuleFor(x => x.Surname).InclusiveBetween("aa", "zz"));
		var result = validator.Validate(new Person { Surname = "zz" });
		result.IsValid.ShouldBeTrue();
	}

	[Fact]
	public void When_the_to_is_smaller_than_the_from_then_the_validator_should_throw_for_strings() {
		Assert.Throws<ArgumentOutOfRangeException>(() => RangeValidatorFactory.CreateInclusiveBetween<Person, string>("ccc", "aaa"));
	}

	[Fact]
	public void When_the_validator_fails_the_error_message_should_be_set_for_strings() {
		var validator = new TestValidator(v => v.RuleFor(x => x.Surname).InclusiveBetween("bbb", "zzz"));
		var result = validator.Validate(new Person { Surname = "aaa" });
		result.Errors.Single().ErrorMessage.ShouldEqual("'Surname' must be between bbb and zzz. You entered aaa.");
	}

	[Fact]
	public void To_and_from_properties_should_be_set_for_strings() {
		var validator = RangeValidatorFactory.CreateInclusiveBetween<Person, string>("a", "c");
		validator.From.ShouldEqual("a");
		validator.To.ShouldEqual("c");
	}

	[Fact]
	public void Validates_with_nullable_when_property_is_null() {
		var validator = new TestValidator(v => v.RuleFor(x => x.NullableInt).InclusiveBetween(1, 5));
		var result = validator.Validate(new Person());
		result.IsValid.ShouldBeTrue();
	}

	[Fact]
	public void Validates_with_nullable_when_property_not_null() {
		var validator = new TestValidator(v => v.RuleFor(x => x.NullableInt).InclusiveBetween(1, 5));
		var result = validator.Validate(new Person { NullableInt = 10 });
		result.IsValid.ShouldBeFalse();
	}

	[Fact]
	public void When_the_value_is_between_the_range_specified_by_icomparer_then_the_validator_should_pass() {
		var validator = new TestValidator(v => v.RuleFor(x => x.Address).InclusiveBetween(
			new Address() { Line1 = "3 Main St." },
			new Address() { Line1 = "10 Main St." },
			new StreetNumberComparer
				()));
		var result = validator.Validate(new Person { Address = new Address() { Line1 = "5 Main St." } });
		result.IsValid.ShouldBeTrue();
	}

	[Fact]
	public void When_the_value_is_smaller_than_the_range_by_icomparer_then_the_validator_should_fail() {
		var validator = new TestValidator(v => v.RuleFor(x => x.Address).InclusiveBetween(
			new Address() { Line1 = "3 Main St." },
			new Address() { Line1 = "10 Main St." },
			new StreetNumberComparer()));
		var result = validator.Validate(new Person { Address = new Address() { Line1 = "1 Main St." } });
		result.IsValid.ShouldBeFalse();
	}

	[Fact]
	public void When_the_value_is_larger_than_the_range_by_icomparer_then_the_validator_should_fail() {
		var validator = new TestValidator(v => v.RuleFor(x => x.Address).InclusiveBetween(
			new Address() { Line1 = "3 Main St." },
			new Address() { Line1 = "10 Main St." },
			new StreetNumberComparer()));
		var result = validator.Validate(new Person { Address = new Address() { Line1 = "11 Main St." } });
		result.IsValid.ShouldBeFalse();
	}
}
