#r "nuget: FSharp.Compiler.Service"

open FSharp.Compiler.SourceCodeServices
open FSharp.Compiler.Text

// Create an interactive checker instance
let checker = FSharpChecker.Create()

/// Get untyped tree for a specified input
let getUntypedTree (file, input) =
    // Get compiler options for the 'project' implied by a single script file
    let projOptions, errors =
        checker.GetProjectOptionsFromScript(file, input)
        |> Async.RunSynchronously

    let parsingOptions, _errors =
        checker.GetParsingOptionsFromProjectOptions(projOptions)

    // Run the first phase (untyped parsing) of the compiler
    let parseFileResults =
        checker.ParseFile(file, input, parsingOptions)
        |> Async.RunSynchronously

    match parseFileResults.ParseTree with
    | Some tree -> tree
    | None -> failwith "Something went wrong during parsing!"

open FSharp.Compiler.SyntaxTree

/// Walk over a pattern - this is for example used in
/// let <pat> = <expr> or in the 'match' expression
let rec visitPattern =
    function
    | SynPat.Wild (_) -> printfn "  .. underscore pattern"
    | SynPat.Named (pat, name, _, _, _) ->
        visitPattern pat
        printfn "  .. named as '%s'" name.idText
    | SynPat.LongIdent (LongIdentWithDots (ident, _), _, _, _, _, _) ->
        let names =
            String.concat "." [ for i in ident -> i.idText ]

        printfn "  .. identifier: %s" names
    | pat -> printfn "  .. other pattern: %A" pat


/// Walk over an expression - let expression
/// contains pattern and two sub-expressions
let rec visitExpression =
    function
    | SynExpr.Const (con, _) ->
        match con with
        | SynConst.Int32 (value) -> printfn "%i" value
        | _ -> printfn " - not supported const: %A" con
    | SynExpr.App (_, _, funExpr, argExpr, _) ->
        visitExpression funExpr
        visitExpression argExpr
    | expr -> printfn " - not supported expression: %A" expr



/// Walk over a list of declarations in a module. This is anything
/// that you can write as a top-level inside module (let bindings,
/// nested modules, type declarations etc.)
let visitDeclarations decls =
    for declaration in decls do
        match declaration with
        | SynModuleDecl.Let (_, bindings, _) ->
            // let <pat> = <expr>
            for binding in bindings do
                let (Binding (_, _, _, _, _, _, _, pat, _, expr, _, _)) = binding
                visitPattern pat
                visitExpression expr
        | _ -> printfn " - not supported declaration: %A" declaration


let visitModulesAndNamespaces modulesOrNss =
    for moduleOrNs in modulesOrNss do
        let (SynModuleOrNamespace (lid, _, _, decls, _, _, _, _)) = moduleOrNs
        visitDeclarations decls


let input = "er = 2"

let file =
    "/home/andreas/master/dat259/project/src/Test.fsx"

// Get the AST of sample F# code
let tree =
    getUntypedTree (file, SourceText.ofString input)

// Extract implementation file details
match tree with
| ParsedInput.ImplFile (implFile) ->
    let (ParsedImplFileInput (_, _, _, _, _, modules, _)) = implFile
    visitModulesAndNamespaces modules
| _ -> failwith "file not supported"
