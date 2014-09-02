<Query Kind="Program" />

void Main()
{
	//var tests = new SimpleClassTests();
	
	var tests = new CalculatorTests();
	tests.RunTests();
}


// Create a simple String calculator with a method int Add(string numbers)
// The method can take 0, 1 or 2 numbers, and will return their sum (for an empty string it will return 0) for example “” or “1” or “1,2”
// Remember to solve things as simply as possible so that you force yourself to write tests you did not think about
// Remember to refactor after each passing test
class Calculator
{

	private List<char> _delimiters = new List<char> { ',', '\n' };
	
	public int AddNumber(string numbers)
	{
		var result = int.Parse(numbers);
		if (result < 0)
			throw new ArgumentException(string.Format("Negative Numbers are not allowed: {0}", result));
		return result;		
	}
	
	public int AddNumbers(string numbers)
	{
		if (numbers.StartsWith("//"))
		{
			_delimiters.Add(numbers[2]);
			numbers = numbers.Substring(4);
		}
		var result = 0;
		var containsNegativeNumber = false;
		foreach (var numberAsString in numbers.Split(_delimiters.ToArray()))
		{
			var numbersAsInteger = int.Parse(numberAsString);
			if (numbersAsInteger < 0) containsNegativeNumber = true;
			result += numbersAsInteger;
		}
		//var numberSplit = numbers.Split(_delimiters.ToArray());
		if (containsNegativeNumber)
		{
			throw new ArgumentException(string.Format("Negative Numbers are not allowed: {0}", string.Join(",", numbers.Split(_delimiters.ToArray()).Where (n => int.Parse(n) < 0))));
		}
		return result;
	}
	
	public int Add(string numbers)
	{
		if (string.IsNullOrEmpty(numbers)) return 0;
		
		//if (numbers.Contains("-")) throw new ArgumentException("Negative Numbers are not allowed");
		
		if (_delimiters.Any (n => numbers.Contains(n)) || numbers.StartsWith("//")) return AddNumbers(numbers);	
				
		return AddNumber(numbers);	
	}
}

class CalculatorTests : UnitTestBase
{
	private Calculator _calculator;
	public void SetupTest()
	{
		// Arrange
		_calculator = new Calculator();
	}

	// Start with the simplest test case of an empty string and move to 1 and two numbers
	[Test]
	public void Given_an_empty_string_When_adding_Then_return_0()
	{		
		// Act
		var result = _calculator.Add(string.Empty);
		
		// Assert
		Assert.AreEqual(0, result);
	}

	[TestCase("1", 1)]
	[TestCase("2", 2)]
	[TestCase("3", 3)]
	public void Given_a_single_number_When_adding_Then_return_input_number(string number, int expected)
	{
		// Act
		var result = _calculator.Add(number);
		
		// Assert
		Assert.AreEqual(expected, result);
	}
	
	[Test]
	public void Given_two_numbers_When_adding_Then_return_the_sum()
	{
		// Act
		var result = _calculator.Add("1,2");
		
		// Assert
		Assert.AreEqual(3, result);
	}
	
	// Allow the Add method to handle an unknown amount of numbers
	[TestCase("1,2,3,4,5,6", 21)]
	[TestCase("7,8,9,10,11", 45)]
	public void Given_an_unknown_amount_of_numbers_When_adding_Then_return_the_sum(string numbers, int expected)
	{
		// Act
		var result = _calculator.Add(numbers);
		
		// Assert
		Assert.AreEqual(expected, result);
	}
	
	// Allow the Add method to handle new lines between numbers (instead of commas).
	// the following input is ok:  “1\n2,3”  (will equal 6)	
	[TestCase("1\n2", 3)]
	[TestCase("1\n2,3", 6)]
	public void Given_a_list_of_numbers_with_new_lines_When_adding_Then_return_the_sum(string numbers, int expected)
	{
		// Act
		var result = _calculator.Add(numbers);
		
		// Assert
		Assert.AreEqual(expected, result);
	}
	
	// the following input is NOT ok:  “1,\n” (not need to prove it - just clarifying)	
	[Test]
	[ExpectException(typeof(FormatException))]
	public void Given_an_invalid_string_format_When_adding_Then_throw_FormatException()
	{
		// Act
		var result = _calculator.Add("1,\n");
	}

	// Support different delimiters
	// to change a delimiter, the beginning of the string will contain a separate line that looks like this:   “//[delimiter]\n[numbers…]” for example “//;\n1;2” should return three where the default delimiter is ‘;’ .
	// the first line is optional. all existing scenarios should still be supported
	[Test]
	public void Given_a_custom_delimiter_When_adding_Then_return_the_sum()
	{
		// Act
		var result = _calculator.Add("//;\n1;2");
		
		// Assert
		Assert.AreEqual(3, result);
	}
	
	// Calling Add with a negative number will throw an exception “negatives not allowed” - 
	// and the negative that was passed.
	[Test]
	[ExpectException(typeof(ArgumentException), Message="Negative Numbers are not allowed: -1")]
	public void Given_a_negative_number_When_adding_Then_throw_exception()
	{
		// Act
		var result = _calculator.Add("-1");
	}
	
	// if there are multiple negatives, show all of them in the exception message
	[Test]
	[ExpectException(typeof(ArgumentException), Message="Negative Numbers are not allowed: -1,-2")]
	public void Given_multiple_negative_numbers_When_adding_Then_throw_exception()
	{
		// Act
		var result = _calculator.Add("-1,-2");
	}	
	
	[Test]
	[ExpectException(typeof(ArgumentException), Message="Negative Numbers are not allowed: -1,-3")]
	public void Given_multiple_negative_numbers_and_a_positive_number_When_adding_Then_throw_exception()
	{
		// Act
		var result = _calculator.Add("-1,2,-3");
	}		
}

/*



Advanced:
Numbers bigger than 1000 should be ignored, so adding 2 + 1001  = 2
Delimiters can be of any length with the following format:  “//[delimiter]\n” for example: “//[***]\n1***2***3” should return 6
Allow multiple delimiters like this:  “//[delim1][delim2]\n” for example “//[*][%]\n1*2%3” should return 6.
make sure you can also handle multiple delimiters with length longer than one char

*/

class SimpleClass
{
	public int Method1(string input)
	{
		return 1;
	}
}

class SimpleClassTests : UnitTestBase
{

	private SimpleClass _simpleClass;

	public void SetupTest()
	{
		_simpleClass = new SimpleClass();
	}

	[Test]
	public void Method1_AnyInput_Returns1()
	{
		var result = _simpleClass.Method1("somestring");		
		Assert.AreEqual(1, result);
	}
	
	[Test]
	public void Method1_AnyInput_Fails()
	{
		var result = _simpleClass.Method1("somestring");		
		Assert.AreEqual(0, result);
	}	
	
	[TestCase("some input", 0)]
	[TestCase("some other input", 1)]
	public void Method1_TestCaseTest_DoesBoth(string input, int expected)
	{
		var result = _simpleClass.Method1(input);
		Assert.AreEqual(expected, result);
	}
	
	[Test]
	[ExpectException(typeof(ArgumentException),Message="Wow")]
	public void Method1_TestException_Succeeds() 
	{
		throw new ArgumentException("Wow");
	}

	[Test]
	[ExpectException(typeof(ArgumentException),Message="Wow")]
	public void Method1_TestException_Fails() 
	{
		throw new IOException("Wow");
	}
}

[AttributeUsage(AttributeTargets.Method)]
class TestAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple=true)]
class TestCaseAttribute : Attribute
{
	public object[] Parameters { get; private set; }
	
	public TestCaseAttribute(params object[] parameters)
	{
		Parameters = parameters;
	}
}

[AttributeUsage(AttributeTargets.Method)]
class ExpectExceptionAttribute : Attribute
{
	public Type ExceptionType { get; private set; }
	public string Message { get; set; }
	
	public ExpectExceptionAttribute(Type exceptionType)
	{
		ExceptionType = exceptionType;
	}
	
	public bool Equals(Exception exception) 
	{
		if (string.IsNullOrEmpty(exception.Message))
		{
			return (ExceptionType == exception.GetType()) && Message.Equals(exception.Message);
		}
		else
		{
			return (ExceptionType == exception.GetType());
		}
	}
}

class Assert
{
	public bool Passed { get; private set; }
	public string Message { get; private set; }
	
	public Assert()
	{
		Reset();
	}
	
	public void Reset() 
	{
		Passed = true;
		Message = string.Empty;	
	}
	
	public void AreEqual<T>(T expected, T actual) where T : IEquatable<T>
	{
		if (!Passed) return;
		
		if (!expected.Equals(actual))
		{
			Passed = false;
			Message = string.Format("Expected: {0} But was: {1}", expected, actual);
		}
	}
	
	public void WriteResults(string methodName) 
	{
		if (Passed) 
		{
			Console.WriteLine("Passed: {0}", methodName);
		}
		else
		{
			//Console.WriteLine("Failed: {0} - {1}", methodName, Message);
			Util.Highlight(string.Format("Failed: {0} - {1}", methodName, Message)).Dump();
		}		
	}
	
	public void FailThroughException(Exception exception)
	{
		if (!Passed) return;
		
		Passed = false;
		Message = string.Format("{0} was not handled", exception.GetType());
	}
	
	public void FailThroughExpectedExceptionNotHandled(ExpectExceptionAttribute expectExceptionAttribute) 
	{
		if (!Passed) return;
		
		Passed = false;
		Message = string.Format("Expected {0} with message: {1}", expectExceptionAttribute.ExceptionType, expectExceptionAttribute.Message);
	}
}

abstract class UnitTestBase 
{
	protected Assert Assert { get; private set; }
	private int _passed = 0;
	private int _failed = 0;
	private int TestCount { get { return this.GetType().GetMethods().Count(m => m.IsDefined(typeof(TestAttribute), false) || m.IsDefined(typeof(TestCaseAttribute), false)); } }
	
	public UnitTestBase()
	{
		Assert = new Assert();
	}
	
	public void RunTestMethod(MethodInfo testMethod, MethodInfo setupMethod, object[] parameters)
	{
		Assert.Reset();
		if (setupMethod != null)
		{
			this.GetType().InvokeMember(setupMethod.Name, BindingFlags.InvokeMethod, null, this, null);
		}
		var expectExceptionAttribute = testMethod.GetCustomAttributes(typeof(ExpectExceptionAttribute), false).Cast<ExpectExceptionAttribute>().FirstOrDefault();
		var exceptionHandled = false;
		try
		{
			this.GetType().InvokeMember(testMethod.Name, BindingFlags.InvokeMethod, null, this, parameters);	
		}
		catch(TargetInvocationException targetInvocationException)
		{
			var exception = targetInvocationException.InnerException;
			//exception.Dump();
			//expectExceptionAttribute.Dump();
			if (expectExceptionAttribute == null || !expectExceptionAttribute.Equals(exception))
			{
				Assert.FailThroughException(exception);				
			}
			exceptionHandled = true;
		}
		if (expectExceptionAttribute != null && !exceptionHandled)
		{
			Assert.FailThroughExpectedExceptionNotHandled(expectExceptionAttribute);
		}
	}
	
	public void RunTests()
	{	
		var methods = this.GetType().GetMethods();
		var setupMethod = methods.Where (m => m.Name.Equals("SetupTest")).FirstOrDefault();
		foreach (var method in methods.Where (m => m.IsDefined(typeof(TestAttribute), false) || m.IsDefined(typeof(TestCaseAttribute), false) ))
		{
			Assert.Reset();
			if (method.IsDefined(typeof(TestAttribute), false))
			{
				RunTestMethod(method, setupMethod, null);
				if (!Assert.Passed) _failed++; else _passed++;
			}
			else if (method.IsDefined(typeof(TestCaseAttribute), false))
			{
				var failed = false;
				var testCaseAttributes = method.GetCustomAttributes(typeof(TestCaseAttribute), false).Cast<TestCaseAttribute>();
				foreach (var testCaseAttribute in testCaseAttributes)
				{
					RunTestMethod(method, setupMethod, testCaseAttribute.Parameters);	
					if (!Assert.Passed) 
					{
						failed = true;
						break;
					}					
				}
				if (failed) _failed++; else _passed++;
			}
			Assert.WriteResults(method.Name);
		}
		
		Console.WriteLine();
		
		if (_failed == 0)
		{
			Console.WriteLine("Failed: {0}", _failed);
		}
		else 
		{
			Util.Highlight(String.Format("Failed: {0}", _failed)).Dump();
		}
		
		
		Console.WriteLine("Passed: {0}", _passed);		
		Console.WriteLine("Total: {0}", TestCount);
	}
}

// Quick exercise for practicing TDD based on Roy Osherove's
// description here: http://osherove.com/tdd-kata-1
// https://www.youtube.com/watch?v=BTWXQBFK0XQ&list=PL3D3F4B7C71FF6AA0&index=4

