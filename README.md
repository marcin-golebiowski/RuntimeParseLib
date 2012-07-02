RuntimeParseLib
===============

The goal of this project is to develop a library that simplifies the dynamic generation of lexers and parsers at runtime, and builds upon these foundations to provide a complete text processing system that uses serialization and xml configuration to define records and record reading behavior. So far, the library only has support for the generation of some simple types of lexers, but as of that accomplishment the project is less than a week old.

So far, this is what the library is capable of:
1. Using a very simple object model, a Lsm (or lexer state machine) document can be built at runtime with state, match rule, and action nodes, defining the behavior of a state machine. This document can then (also at runtime) be compiled into a lexer function. You can then pass an instance of TextReader, and an instance of List<LsmToken> to the function, it will scan the text, populate the list with tokens, and return the state that the lexer was at last.
2. Simple text representing the hiearchy of States, Rules, and Actions can be generated.
3. An LsmBuilder class that makes it easier to build up the state pathways. It supports only a very limited set of token at the moment, but this class will be scrapped very shortly in favor of a much better API for easily generating state pathways.

The near future involves the following plans. Remember, this is only the near future and does not touch upon the many new features that I plan to implement.

1. Replace LsmBuilder class with a much better one that facilitates the building up tress of Lsm objects in a way that works and is easy to use.
2. Add error handling to the lexer expressions to provide information about invalid tokens.