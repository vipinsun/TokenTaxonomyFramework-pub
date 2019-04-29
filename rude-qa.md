# Rude Q/A

Questions will be by section, the Token Taxonomy Framework (TTF) and the Token Working Group.

## Token Taxonomy Framework (TTF)

- How do I use the framework?

 >The framework is as much a process as it is a set of artifacts. Users of the framework should be business and technology participants that understand the business requirements that can run a *Taxonomy Workshop* session to identify existing artifacts in the framework they can employ to define their token and then create new behavior artifacts for those that do not exist yet.  These new artifacts are then contributed back to the framework to be included in the library for the benefit of others.

- Do I have to contribute my artifacts to the framework?

 >Yes, this is the collective benefit for frameworks.  Contributing your artifacts ensure that the behavior you described can be understood and implemented by others.  This understanding will allow for common issue resolution with regulator bodies or collaborations to accelerate since they don't have to start at ground zero.  Also, if the behavior you described in your artifact does not have any technical implementations, attracting or contracting another party to implement the behavior for you becomes much easier.

- What is metadata for?

 >The taxonomy has metadata included in a framework artifact.  This metadata is used to allow external tools to navigate, visualize and extract reports from the taxonomy framework GitHub structure.

- What do these symbols mean?

 >The taxonomy uses symbols to represent concepts or variables, like in a mathematical formula.  It is not intended to be used by the typical business user of the framework, but for developers and automated tooling to navigate the framework's GitHub structure to pull and present data for the typical user to understand.  For example, a fungible token has a symbol of  *&tau;<sub>F</sub>*, that is recorded as metadata in the artifact for a fungible token.  A taxonomy exploration tool or a token design tool can use this symbol to pull from the GitHub artifact things like common names, descriptions, analogies, properties, etc. from the artifact to visually and in easy to understand language.  Since it uses the symbol to pull from the taxonomy, it's content is always in sync with the latest version of the taxonomy.  These symbols also let you describe a complex token using a formula and have tooling be able to graphically represent the token definition.

## Token Working Group

- Why do we need another standards group for tokens, isn't there a bunch of them already?

 >Yes, there are many token standards bodies that have popped up lately.  But these groups are either standardizing for a specific platform or industry and are unlikely to be compatible much less understandable when attempting to use together.  The TTF generates high-level "standard" artifacts that capture definitions for business and legal functionality using language everyone can understand based on a common set of terms and metadata that provides a foundation for building a large library of reusable artifacts in the framework to accelerate token adoption.  We believe that the TTF can provide commonality across the other standards efforts and help bridge gaps in implementations and light up deficiencies in the ecosystem as well.

- I thought this was an Ethereum organization, what is up with all these other blockchain implementations and competitors?

 >The EEA's starts with the word Enterprise. Enterprises face a reality of needing to support multiple blockchain and ledger platforms as well as multiple consortiums as we all progress through this distributed journey to change the world. With that said, there is tremendous value for everyone to establish common ground between platforms and consortiums that will need to inter-operate at some point. To approach this reality, it is tempting to dive right into technical solutions to start bridging this gap, but such efforts lead to a false summit.  The task at hand has many constituents including regulatory agencies, legislative support and eventually lower level standards for implementations.  But before any of these constituents are brought along, we MUST establish a baseline and establish a core set of high level business terms and definitions to be allow for all of these constituents to speak the same language.  This common language does not exist, but this group will create the framework for this common language by being inclusive to all, prevent repetition and collisions and enable for the summit to be mounted to be correctly identified.

- Is this group defining Tokens?

 >No, this group is creating the taxonomy framework that is used to define tokens.  The framework establishes a common language of terms and definitions, education materials to allow anyone to learn the language and a set of basic artifact templates and a central GitHub repository to host the taxonomy framework and the tokens that the community at large defines.  This group is providing the tools, accessible to anyone, to be able to understand and define or discover a new token implementation.

- Who benefits from this work group?

 >Everyone. Existing token implementations can apply the taxonomy to create a common definition of their token, add any missing artifacts and then link their token implementation to the taxonomy definition so it is discoverable by any user of the framework or tools derived from it.  Platform providers can use the taxonomy in their SDK documentation and support integration to register tokens their customers create with the taxonomy. These platform providers can use the taxonomy to identify their strengths and weaknesses to improve and differentiate their platform in a way that customers can understand. Everyone will benefit from a standard regulatory conversation, one that doesn't require translators and hours of establishing a common understanding of terms and definitions. And finally, users will benefit from the educational materials, tools and discovery allowing them to make informed choices in implementation, clearly articulate requirements that implementors can understand.