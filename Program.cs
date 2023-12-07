using System;
using VDS.RDF;
using VDS.RDF.Nodes;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Update;

class Program
{
    static void Main()
    {
        // Create an in-memory RDF graph and load data
        // TripleStore tripleStore = new TripleStore();
        // Graph graph = new Graph();
        // LoadRdfData(graph);
        // tripleStore.Add(graph);

        // // Execute a SPARQL query to display initial data
        // ExecuteQuery(graph, @"
        //     PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
        //     PREFIX foaf: <http://xmlns.com/foaf/0.1/>

        //     SELECT ?wall ?id ?size ?color
        //     WHERE {
        //         ?wall rdf:type foaf:Wall ;
        //               foaf:id ?id ;
        //               foaf:size ?size ;
        //               foaf:color ?color .
        //     }
        // ");

        // // Perform a SPARQL Update to modify data
        string myQuery ="PREFIX dc: <http://purl.org/dc/elements/1.1/>\r\nDELETE DATA FROM <http://example/bookStore> {<http://example/book3>  dc:title  \"Fundamentals of Compiler Desing\" }"; 
        string sparqlUpdate = @"
            PREFIX foaf: <http://xmlns.com/foaf/0.1/>

            DELETE WHERE {
                ?wall rdf:type foaf:Wall ;
                      foaf:id ?id ;
                      foaf:size ?size ;
                      foaf:color ?color .
            }

            INSERT {
                ?wall rdf:type foaf:Wall ;
                      foaf:id 123 ;
                      foaf:size 100 ;
                      foaf:color 'Green' .
            }

            WHERE {
                ?wall rdf:type foaf:Wall ;
                      foaf:id 456 ;
                      foaf:size 50 ;
                      foaf:color 'Blue' .
            }
        ";

        GetQuery(myQuery);

        // Console.WriteLine("SPARQL UPDATE Query:");
        // Console.WriteLine(sparqlUpdate);
        // Console.WriteLine();

        // ExecuteUpdate(tripleStore, sparqlUpdate);

        // // Execute a SPARQL query to display modified data
        // ExecuteQuery(graph, @"
        //     PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
        //     PREFIX foaf: <http://xmlns.com/foaf/0.1/>

        //     SELECT ?wall ?id ?size ?color
        //     WHERE {
        //         ?wall rdf:type foaf:Wall ;
        //               foaf:id ?id ;
        //               foaf:size ?size ;
        //               foaf:color ?color .
        //     }
        // ");
    }

    static void LoadRdfData(IGraph graph)
    {
        // Example RDF data in Turtle syntax
        string rdfData = @"
            @prefix rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .
            @prefix foaf: <http://xmlns.com/foaf/0.1/> .

            <http://example.org/Wall01> rdf:type foaf:Wall01 ;
                foaf:id 123 ;
                foaf:size 50 ;
                foaf:color 'Blue' .

            <http://example.org/Wall02> rdf:type foaf:Wall02 ;
                foaf:id 456 ;
                foaf:size 75 ;
                foaf:color 'Red' .
        ";

        // Load RDF data from the string
        graph.LoadFromString(rdfData);
    }

   static void ExecuteQuery(IGraph graph, string sparqlQuery)
{
    SparqlQueryParser parser = new SparqlQueryParser();
    SparqlQuery query = parser.ParseFromString(sparqlQuery);

    SparqlResultSet? results = graph.ExecuteQuery(query) as SparqlResultSet;

    Console.WriteLine("Query Results:");
    PrintResults(results);
    Console.WriteLine();
}

private static SparqlQuery GetQuery(string queryString)
{
    SparqlQueryParser parser = new SparqlQueryParser();
    SparqlQuery query = parser.ParseFromString(queryString);
    return query;
}



    static void ExecuteUpdate(TripleStore tripleStore, string sparqlUpdate)
    {
        SparqlUpdateCommandSet updateCommands = new SparqlUpdateParser().ParseFromString(sparqlUpdate);

        foreach (var command in updateCommands.Commands)
        {
            tripleStore.ExecuteUpdate(command);
        }

        Console.WriteLine("Update Applied\n");
    }

    static void PrintResults(SparqlResultSet? results)
{
    if (results != null)
    {
        foreach (SparqlResult result in results)
        {
            Console.WriteLine($"Wall: {result["wall"]} - ID: {result["id"].AsValuedNode().AsInteger()} - Size: {result["size"].AsValuedNode().AsInteger()} - Color: {result["color"]}");
        }
    }
    else
    {
        Console.WriteLine("Query did not return results.");
    }
}

}
