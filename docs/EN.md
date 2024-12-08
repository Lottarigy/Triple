# DOCUMENTATION
# Polylingualism
Perhaps it's worth starting with polylingualism, because it is what decides how exactly you want to write code on triple. 
Polylingualism allows you to write code with localization. In other words, the triple vocabulary supports several human languages. 
At the time of version 1.0, the interpreter supports three localizations.

English localization :
```haskell
: Stream() -> num =>
     return 0
```
Belarusian localization:
```haskell
: Струмень() -> лік =>
    вярнуць 0
```
Russian localization:
```haskell
: Поток() -> число =>
    вернуть 0
```
The language constructs will be discussed in the following sections.

# Input/Output
The tradition of getting acquainted with any new programming language is to write a program that outputs Hello World. 
Usually, this is the program with which any programmer begins his journey.
In Triple, text can be output using two functions - `>>Output()` and `>>Outputln()`. 
The second, unlike the first, after outputting a line to the console, moves to the next line.

Let's look at an example:

```haskell
: Stream() -> num =>
    >>Output("Hello world!")
    return 0
```

This is what we will see in the console:

`Hello world!`

`The program exited with code: 0`

If you need to enter data, use the function `>>Inputln`.

Let's look at an example:

First, let's output what we entered, otherwise the interpreter will return an error.
```haskell
: Stream() -> num =>
    >>Output(>>Inputln())
    return 0
```
In the console we will see a blinking underscore, the interpreter will wait for the data to be entered. 
Now the text you entered will be displayed!



# Data types
In triple , due to static typing, you need to explicitly declare types.

### Triple types:

* number type `num`

* string type `stg`

* character type `chr`

* bool type `bool` (tr,fls)

### There are also complex types:
* set
* function


# Variables 
In the previous section we talked about data types, so their main use is to create variables. 
To create variables, use the following construction: 
`let <name> -> <type> = <value>`<br> 
You can assign any names to variables, the main thing is that they do not repeat 

But in addition to creating them, you can also use them.

```haskell
: Stream() -> num =>
    let var1 -> stg = "Hello World!"
    >>Output(var1)
    return 0
```
Console: `Hello World!`

In the case of assignment, the syntax is trivial:

```haskell
: Stream() -> num =>
    let var2 -> chr = 'D'
    var2 = 'F'
    >>Output(var2)
    return 0
```
Console: `F`

# Operators
Triple supports basic mathematical operators:

* Sum `+`
* Difference `-`
* Product `*`
* Division `/`
* Exp `^`
* Mod `%`

Operations work only with numeric data types and string data types in case of concatenation.

```haskell
: Stream() -> num =>
    let a -> num = 10
    let b -> num = 2
    >>Output((a * (4 + 5))/b)
    return 0
```

# Branching
In the language, branching is defined as follows: `if <condition> => <action>
 if <condition> => <action>
 otherwise => <action>``

Example:

```haskell
: Stream() -> num =>
    if 1 > 2 => >>Output(1)
    elif 1 == 1 => >>Output(2)
    else => >> >>Output(3)
    return 0
```

Console: `2`

Assertion blocks are also supported:
 

```haskell
: Stream() -> num =>
    let a -> num = 1
    let b -> num = 2
    let c -> num = 3
    
    if a > 0 =>
        | a = a + 1
        | if a == b => b = b + 1
        | if b == c => >>Output("Done") ]
    else => >>Output("Unexpected arithmetic")

    return 0
```

Console: `Done`.

If using blocks, you must put a `|` symbol for each statement. After the block is finished, you should put `]`.

# Loops
Loops are an integral part of any programming language, triple is no exception. The language supports two types of loops. 

The first kind of loop is that it first checks if the condition is true and then executes something while it is true:

```haskell
: Stream() -> num =>
    while 1 > 0 => >>Output(1)
    return 0
```
Console: `11111111111...`

In this case we have nothing and nowhere is updated, one is always greater than zero, the condition remains static, hence the loop will turn out to be infinite. To avoid this, let's look at an example:

```haskell
: Stream() -> num =>
    let a -> num = 0
    while a <= 10 =>
        | >>Output(a) 
        | a = a + 1 ]
```

Console: `012345678910`.

First we create a variable, then we compare this variable with 10, since the value of the variable is zero, compare 0 <= 10? - yes, output 0, further we update the variable `a`: add one to it, again compare 1 <= 10? - yes, output 1, then update the variable again, and so on by the same analogy until the variable is equal to 11: compare 11 <= 10? - no, the loop breaks and the program is terminated. It is worth noting that all loops support the same statement block as branching: `| ]`.

There is one more kind of loop: `do-while`. Its essence is the same as that of a regular `while' loop, except that this type of loop first does something (executes) and then only compares it. You can see the difference in principle on the examples:

loop `while`

```haskell
: Stream() -> num =>
    let g -> num = 4
    
    while g < 3 => >>Output(g)

    return 0
```

It won't output anything in the console.


loop `do-while`

```haskell
: Stream() -> num =>
    let g -> num = 4
    
    do => >>Output(g)
    while g < 3 ]

    return 0
```

Console: `4`

The difference is that the `do-while` loop manages to output the value before the value comparisons occur, while the `while` loop immediately compares the values and outputs nothing because the condition is false.

# Collections
There are no arrays in Triple, instead there are collections. 
A collection set is a complex data type, it allows you to store multiple values in a variable, and the values can be of any data type. Let's look at an example:

```haskell
: Stream() -> num =>
    let A -> set = {1,2,3,4,5}
    >>Output(A[1]) 

    return 0
```

Console: `2`

You need to call the elements of a set through the construct: `<set name>[element by index]`. Set indexing starts from zero, i.e. the first element of the set always has a zero index, the second element has an index of one, and so on. 

A set supports the `<set_name>[number]` mechanism to output the total number of its elements. Consider the example:

```haskell
: Stream() -> num =>
    let B -> set = {'a', 'b', 'c'}
    let a -> num = 0
    while a < B[count] =>
        | >>Output(B[a])
        | a = a + 1 ]
    return 0
```

Console: `abc`.

Here the loop is needed to output all the elements of the set to the console. It is executed as long as `a` is less than the number of elements of the set `B`, i.e. first check 0 < 3? then 1 < 3? and the last check 3 < 3? Here the loop breaks.

# Functions 
Functions are subroutines and complex data types. They have their own local scope, in other words, their data is available only within this scope, that's why you get a subroutine. Functions are defined by the first character: `:`

```haskell
: hello() -> stg =>
    | let result -> stg = "Hi"
    | return result ]
```  

It won't print anything in the console.

A function is math notation oriented, it must return something, in this example if the function is treated as a math function, it takes nothing and returns a string, so you must write the keyword `return` at the end of the function.

Functions are global statements, hence they can be declared at the `Stream` function level, but cannot be declared inside a stream.

To utilize the potential of functions, they must be called. If we call the `Hello()` function in a stream, we can see the result of its execution in the console:

```haskell
: Hello() -> stg =>
    | let result -> stg = "Hi"
    | return result ]

: Stream() -> num =>
    >>Output(>>Hello())
    return 0
```  

Console: `Hello`

The function is called via `>>`. Since the function returned a result with a `stg` type, when it is called it essentially becomes a value with a string data type, we could have written:

```haskell
: Hello() -> stg =>
    | let result -> stg = "Hi"
    | return result ]

: Stream() -> num =>
    let a -> stg = >>Hello()
    >>Output(a)
    return 0
```

Console : `Hello`

Here are a few rules when declaring functions:
* Functions must be declared with the first capital letter;
* Functions must return something;
* Nested functions do not exist;

Since the `Stream` function is also a function, the rules apply to it as well. A stream can return any data type and expression:

```haskell
: Stream() -> bool =>
     >>Output(0)
     return true 
```
Console: `0`
`the program exited with the code: true`.

Functions can accept parameters of any data type. Let's consider an example:

```haskell
: Sum(a -> num, b -> num) -> num =>
     | return a + b ]

: Stream() -> num =>
     >>Output(>>Sum(2,8))
     return 0
```
Console: `10`

When we call a function, we write values called arguments there. Parameters and arguments are listed comma-separated.

# Procedures
Procedures are a special case of functions. They always return `void`:

```haskell
: Product(a -> num, b -> num) -> void =>
      | >>Output(a * b) ]

: Stream() -> num =>
      >>Product(5,5)
      return 0
```

Console: `25`

Since the function doesn't return anything, it is advisable to call the output function there if we want it to display anything at all.

Procedures are very concise when called:

```haskell
: Stream() -> num =>
     >>Factorial(5)
     return 0

: Factorial(a -> num) -> void =>
     | let result -> num = 1
     | let index -> num = 1
     | while index <= a =>
           | result = result * index 
           | index = index + 1 ]
     | >>Output(result) ]
```
Console: `120`

# Structures
Structs are a shell for functions and are also global statements, meaning that structures can be declared at the function level. 
Structures cannot be nested and cannot be contained in a stream. Structures are defined by the first `::` character and the word `structure'

Example:

```haskell
:: structure Shape
     : Square() -> void => >>Output("This is a square")
     bound
```

A structure is required to end with the `bound` keyword.

Structures can be called using the `<<` symbol:

```haskell
: Stream() -> num =>
     <<Shape >>Circle
     return 0

:: structure Shape
     : Square() -> void => >>Output("This is a square")
     : Circle() -> void => >>Output("This is a circle")
     : Triangle() -> void => >>Output("This is a triangle")
     bound
```

Console: `This is a circle'

Structures are needed to structure code, to break it into blocks. 



