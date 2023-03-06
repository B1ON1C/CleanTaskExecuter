using System.Runtime.Serialization;

namespace CleanTaskExecuter.TaskController.Exceptions
{
    [Serializable]
    public class TasksNotFoundException : Exception
    {
        public TasksNotFoundException(string? message) : base(message)
        {
        }

        public TasksNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TasksNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}