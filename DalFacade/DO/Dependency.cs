﻿namespace DO;
/// <summary>
/// An entity definition describes the dependency between the 2 different tasks
/// </summary>
/// <param name="Id">Dependency ID number </param>
/// <param name="DependentTask">ID number of pending task </param>
/// <param name="DependsOnTask"> Previous assignment ID number</param>
public record Dependency
(

int DependentTask,
int DependsOnTask,
int Id=0
)
{
    /// <summary>
    /// //empty constructor of Dependency
    /// </summary>
    public Dependency() : this(0, 0) { }

   

}
