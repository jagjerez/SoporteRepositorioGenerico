# Generic Repository .Net Core

![](.gitbook/assets/repositorios-genericos-1-.png)

A generic repository is nothing more than the definition of a repository where there are basic search, update, create and delete operations of a model specified by some generic type defined in the class.

## Why should it be generic?

There are many scenarios where repositories are more than copies of itself repeating code many times, for that reason the idea has been created that a single repository is generated where it can be used with all existing models.

## Advantages of using generic repositories?

The advantages of using generic repositories are several but I will name the most important ones.

1. Do not repeat code.
2. Separate the context of the database from the business logic and if the database changes, this does not affect the business logic.

## Disadvantages of using generic repositories?

1. If you need a repository with more specific functions, you must implement a different repository or you must modify the generic repository.

