﻿namespace BO;

public enum WorkerExperience { Beginner, Intermediate, Expert }

public enum Status { Unscheduled, Scheduled, OnTrack, Done }


public enum Filter { ByComplexity, Status,PossibleTaskForWorker,None }

public enum FilterWorker {  ByLevel,Active, Erasable,WithoutTask, None }

public enum ProjectStatus { PlanStage, ScheduleDetermination, ExecutionStage }