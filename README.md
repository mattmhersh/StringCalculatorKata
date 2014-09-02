StringCalculatorKata
====================

The following is a TDD Kata- an exercise in coding, refactoring and test-first, that you should apply daily for at least 15 minutes.

URL Reference: http://osherove.com/tdd-kata-1

Before you start: 
 1. Try not to read ahead. 
 2. Do one task at a time. The trick is to learn to work incrementally.
 3. Make sure you only test for correct inputs. There is no need to test for invalid inputs for this kata 

*String Calculator*

 1. Create a simple String calculator with a method int Add(string
    numbers) 
 2. The method can take 0, 1 or 2 numbers, and will return their sum (for an empty string it will return 0) for example “” or “1” or “1,2”
 3. Start with the simplest test case of an empty string and move to 1 and two numbers
 4. Remember to solve things as simply as possible so that you force yourself to write tests you did not think about
 5. Remember to refactor after each passing test
 6. Allow the Add method to handle an unknown amount of numbers
 7. Allow the Add method to handle new lines between numbers (instead of commas).
 8. Support different delimiters
 9. Calling Add with a negative number will throw an exception “negatives not allowed” - and the negative that was passed.if there are multiple negatives, show all of them in the exception message