# Shibusa.FindTextCli

This is a console application that searches for regular expression matches within the content of files in a directory.
I use it all the time to search across multiple code repos for a bird's-eye-view of where a particular class or variable name appears.

Note that Linux-style paths are used in the examples below. At a Windows command prompt, `/c/code/` is replaced by `c:\code\`\.

## Usage

Pass a directory (`-d <directory>`) and expression (`-e <expression>`) to search content of files. You can add as many expressions as you want.
The default operator applied to the expressions is AND (i.e., expression1 AND expression2 AND expressionN must all be present in a line to match it).
To change this behavior to OR (i.e., expression1 OR expression2 OR expressionN), use the `-o Or` argument.
The expression is treated as case sensitive by default; use `-i` to make the search case insensitive. This will apply to all expressions provided.

```
FindText -d /c/projects/code/ -e "\bassembly\b" -r -l -t -x cs
```

Add `-r` to make the directory search recursive.

Add `-x <extension>` to filter searches of files by file extensions. You can add as many extensions as you want. Providing extensions will limit the files searched and therefore speed up the process.

Add `-l` to see the lines that are matched. Without this, only file names are returned.

Replace `-l` with `-ln` to see the lines with their line numbers. This slows down the process.

Add `-t` to trim the left and right whitespace from the lines before reporting them.

---
### A Note on Expressions

All the following commands are equivalent and will produce identical results.

```
FindText -d /c/projects/code/ -e "^.+?\bassembly\b.+?$" -l -x cs
FindText -d /c/projects/code/ -e "\bassembly\b"         -l -x cs
FindText -d /c/projects/code/ -e "^.+?\bassembly\b"     -l -x cs
FindText -d /c/projects/code/ -e "\bassembly\b.+?$"     -l -x cs
```

The `^.+?` and `.+?$` are required to capture the entire line. When expressions are missing the `^` and `$`, the code will fill them in as needed.
To avoid these manipulations of your expressions, either use the `^` and `$` (at the start and end respectively) in your expressions or use the `-f` to force your expression without manipulation; this will apply to all expressions.

A word of warning with `-f` is that you may not get entire lines back in your results. It may also wreak havoc if you have two or more expressions and are using the AND operator because the scope of the each successive expression check is constrained by its predecessor (sorry, it was intended to do full-line matching).

See `ProcessFileWithAndOperator` for more info on how the AND operator works.

The following are all different expressions (identical to those in the example above) that will yield different results because of the `-f` flag.

```
FindText -d /c/projects/code/ -e "^.+?\bassembly\b.+?$" -l -x cs -f
FindText -d /c/projects/code/ -e "\bassembly\b"         -l -x cs -f
FindText -d /c/projects/code/ -e "^.+?\bassembly\b"     -l -x cs -f
FindText -d /c/projects/code/ -e "\bassembly\b.+?$"     -l -x cs -f
```

---

### FindText --help

```
FindText --directory | --dir | -d <directory> --expression | -e <expression> [--extension | -x <file extension>] [--operator | -o <And | Or>] [--force | -f] [--insensitive | -i] [--recursive | -r] [--show-lines | -l] [--show-line-numbers | -ln] [--trim | -t] [--prefix-file-name | -p] [--use-singleline | -s] [--help | -h | ?]

--directory | --dir | -d <directory>    The directory to search.
--expression | -e <expression>          A regular expression by which to search.
[--extension | -x <file extension>]     Add file extension to extensions searched. When no extensions are provided, all files are searched.
[--operator | -o <And | Or>]            Use 'And' to combine expressions together and use 'Or' if any expression match is desired.
[--force | -f]                          Force your expression to be accepted without manipulation.
[--insensitive | -i]                    Make search case insensitive.
[--recursive | -r]                      Make file searching include subdirectories.
[--show-lines | -l]                     Show the matching lines.
[--show-line-numbers | -ln]             Show the line numbers for matching lines.
[--trim | -t]                           Trim the matching lines in the output.
[--prefix-file-name | -p]               Prefix file names with a newline character.
[--use-singleline | -s]                 Use Singleline (instead of the default Multiline) for regular expressions.
[--help | -h | ?]                       Show this help.

WARNING: The above options may not work together as expected.

Examples:

Find any file containing the whole word 'the' - case-sensitive search:
        FindText -d "/c/repos" -e "\bthe\b"

Same search, but case insensitive:
        FindText - "/c/repos" -e "\bthe\b" -i

Same search, but case insensitive and searching subdirectories:
        FindText -d "/c/repos" -e "\bthe\b" -i -r

Shows lines:
        FindText -d "/c/repos" -e "\bthe\b" -i -r -l

Shows lines with line numbers:
        FindText -d "/c/repos" -e "\bthe\b" -i -r -ln
equivalent to:
        FindText -d "/c/repos" -e "\bthe\b" -i -r -ln -l

Trim the lines in the output:
        FindText -d "/c/repos" -e "\bthe\b" -i -r -ln -t

Find any file containing the whole word 'the' AND the whole word 'best' - case-sensitive search:
        FindText -d "/c/repos" -e "\bthe\b" -e "\bbest\b" -r -ln -t
equivalent to:
        FindText -d "/c/repos" -e "\bthe\b" -e "\bbest\b" -r -ln -t -o And

Find any file containing the whole word 'the' OR the whole word 'best' - case-sensitive search:
        FindText -d "/c/repos" -e "\bthe\b" -e "\bbest\b" -r -ln -t -o Or

Force your expression (to avoid full-line-capturing manipulation):
        FindText -d "/c/repos" -e "First Name: [a-zA-Z]+" -r -ln -f

Same query, but find only first names like 'James' (case insensitive):
        FindText -d "/c/repos" -e "First Name\s+?:\s+?[a-zA-Z]+" -e "\bJames\b" -i -r -ln -f

Find a multi-line block of text (by switching to single-line):
(It seems counter-intuitive, but you use the single-line regular expression option to accomplish this; see: https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-options.)
        FindText -d "/c/repos" -e "<Id>.*</Id>" -s -r -f
```
