# Documentation 
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


