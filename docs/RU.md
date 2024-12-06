# Документация
# Полилингвальность
Начать, пожалуй, стоит с полилингвальности, потому что именно она решает как именно вы хотите писать код на triple.
Полилингвальность позволяет писать код с локализацией. Другими словами лексика triple поддерживает несколько человеческих языков. 
На момент версии 1.0 интерпретатор поддерживает три локализации.

Английская локализация:
```haskell
: Stream() -> num =>
     return 0
```
Белорусская локализация:
```haskell
: Струмень() -> лік =>
    вярнуць 0
```
Русская локализация:
```haskell
: Поток() -> число =>
    вернуть 0
```
Конструкции языка будут рассмотрены в следующих разделах

# Ввод/Вывод
Традицией знакомства с любым новым языком программирования является написание программы вывода Hello World.
Обычно именно с этой программы начинается путь любого программиста.
В Triple текст можно вывести с помощью двух функций - `>>Вывод()` и `>>Выводлн()`.
Вторая, в отличие от первой, после вывода строки в консоль отсутпает на следующую строку.

Давайте рассмотрим пример:

```haskell
: Поток() -> число =>
    >>Вывод("Hello world!")
    вернуть 0
```

Вот что мы увидим в консоли:

`Hello world!`

`Программа завершилась с кодом: 0`

Если нужно ввести данные, то применяется функция `>>Вводлн`.

Рассмотрим пример:

Предварительно выведем то что ввели, иначе интерпретатор выдаст ошибку.
```haskell
: Поток() -> число =>
    >>Вывод(>>Вводлн())
    вернуть 0
```
В консоли мы увидим мигающий символ нижнего подчеркивания, интерпретатор будет ждать пока данные будут введены.
Теперь отобразится текст, который вы ввели!

Поток является функцией-точкой входа в программу. Все конструкции языка (за исключением функций и структур) обязаны находится в Потоке. Поток можно объявлять только один раз.

# Типы данных
В triple из-за статической типизации нужно явно объявлять типы.

### Типы Triple:

* числовой тип `число`

* строковой тип `строка`

* символьный тип `символ`

* логический тип `логика`

### А также существуют сложные типы:
* множество
* функция


# Переменные
В прошлом разделе мы говорили про типы данных, так вот их основным способом применения является создание переменных.
Для создания переменных используется конструкция:
`пусть <имя> -> <тип> = <значение>`<br>
Переменным можно присваивать любые имена, главное чтобы они не повторялись.

Но кроме их создания их можно и использовать.

```haskell
: Поток() -> число =>
    пусть переменная1 -> строка = "Hello World!"
    >>Вывод(переменная1)
    вернуть 0
```
В консоль получим: `Hello World!`

В случае присваивания синтаксис банален:

```haskell
: Поток() -> число =>
    пусть переменная2 -> символ = 'Д'
    переменная2 = 'Ч'
    >>Вывод(переменная2)
    вернуть 0
```
В консоле увидим: `Ч`

# Операции 
В triple поддерживаются базовые математические операции:

* Сумма `+`
* Разность `-`
* Произведение `*`
* Деление `/`
* Возведение в степень `^`
* Деление по модулю `%`

Работают операции только с числовым типом данных и строковым в случае конкатенации.

```haskell
: Поток() -> число =>
    пусть а -> число = 10
    пусть б -> число = 2
    >>Вывод((а * (4 + 5))/б)
    вернуть 0
```

# Ветвление
В языке ветвление определяется следующим образом: `если <условие> => <действие>
 инесли <условие> => <действие>
 иначе => <действие>`

Пример:

```haskell
: Поток() -> число =>
    если 1 > 2 => >>Вывод(1)
    инесли 1 == 1 => >>Вывод(2)
    иначе => >>Вывод(3)
    вернуть 0
```

Консоль: `2`

Также поддерживаются блоки утверждений:
 

```haskell
: Поток() -> число =>
    пусть а -> число = 1
    пусть б -> число = 2
    пусть в -> число = 3
    
    если а > 0 =>
        | а = а + 1
        | если а == б => б = б + 1
        | если б == в => >>Вывод("Выполнено") ]
    иначе => >>Вывод("Непредвиденная арифметика")

    вернуть 0
```

Консоль: `Выполнено`

Если использовать блоки, то нужно ставить символ `|` для каждого утверждения. После того как блок будет закончен, нужно ставить `]`.

# Циклы
Циклы являются неотъемлемой частью любого языка программирования, triple не стал исключением. В языке поддерживаются два вида циклов. 

Первый вид цикла заключается в том, что он сначала проверяет истинность условия, а потом что-то выполняет, пока оно истинно:

```haskell
: Поток() -> число =>
    пока 1 > 0 => >>Вывод(1)
    вернуть 0
```
Консоль: `11111111111...`

В данном случае у нас ничто и нигде не обновляется, единица всегда больше нуля, условие остаётся статическим, отсюда цикл получится бесконечным. Чтобы этого избежать, давайте рассмотрим пример:

```haskell
: Поток() -> число =>
    пусть а -> число = 0
    пока а <= 10 =>
        | >>Вывод(а) 
        | а = а + 1 ]
```

Консоль: `012345678910`

Сначала мы создаём переменную, дальше сравниваем эту переменную с 10, т.к. значение переменной равно нулю, сравниваем 0 <= 10? - да, выводим 0, дальше обновляем переменную `а`: добавляем к ней единицу, снова сравниваем 1 <= 10? - да, выводим 1, дальше снова обновляем переменную, и так дальше по такой же аналогии, пока переменная не станет равна 11: сравниваем 11 <= 10? - нет, цикл обрывается и программа завершается. Стоит отметить, что все циклы поддерживают такой же блок утверждений как и ветвление: `| ]`.

Есть ещё один вид цикла: `исполнять-пока`. Его суть заключается такой же, как и у обычного цикла `пока`, за исключением того, что данный вид цикла сначала что-либо делает(исполняет), а потом только сравнивает. Принципиальное отличие можно посмотреть  на примерах:

цикл `пока`

```haskell
: Поток() -> число =>
    пусть г -> число = 4
    
    пока г < 3 => >>Вывод(г)

    вернуть 0
```

В консоле ничего не выдаст.


цикл `исполнять-пока`

```haskell
: Поток() -> число =>
    пусть г -> число = 4
    
    исполнять => >>Вывод(г)
    пока г < 3 ]

    вернуть 0
```

Консоль: `4`

Отличие в том, что цикл `исполнять-пока` успевает вывести значение, до момента, когда произойдут сравнения значений, в то время, пока цикл `пока` сразу же сравнивает значения и ничего не выводит т.к. условие ложно.

# Коллекции
В Triple нету массивов, вместо них существует коллекции. 
Коллекция множество является сложным типом данных, оно позволяет хранить в переменной несколько значений, причем значения могут быть любых типов данных. Рассмотрим пример:

```haskell
: Поток() -> число =>
    пусть А -> множество = {1,2,3,4,5}
    >>Вывод(А[1]) 

    вернуть 0
```

Консоль: `2`

Вызывать элементы множества нужно через конструкцию: `<имя множества>[элемент по индексу]`. Индексация множества начинается с нуля, то есть у первого элемента множества всегда нулевой индекс, у второго индекс равен единице и так далее. 