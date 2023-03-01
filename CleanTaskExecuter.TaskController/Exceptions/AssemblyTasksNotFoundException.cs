using System.Runtime.Serialization;

namespace CleanTaskExecuter.TaskController.Exceptions
{
    [Serializable]
    public class AssemblyTasksNotFoundException : Exception
    {
        public AssemblyTasksNotFoundException(string? message) : base(message) { }

        public AssemblyTasksNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }

        protected AssemblyTasksNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
