using System;
using System.Runtime.Serialization;
using EnsureThat;

namespace FigureMath.Common.Data.Exceptions
{
    /// <summary>
    /// Represents error that occur when the entity does not exist. 
    /// </summary>
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class without type of the entity and message.
        /// </summary>
        public EntityNotFoundException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class without message. 
        /// </summary>
        /// <param name="entityType">The type of the entity that could not be found.</param>
        public EntityNotFoundException(Type entityType)
        {
            EntityType = EnsureArg.IsNotNull(entityType, nameof(entityType));
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified error message and without type of the entity.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EntityNotFoundException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="entityType">The type of the entity that could not be found.</param>
        public EntityNotFoundException(string message, Type entityType)
            : this(message)
        {
            EntityType = EnsureArg.IsNotNull(entityType, nameof(entityType));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public EntityNotFoundException(string message, Exception inner)
            : base(message, inner)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        /// <param name="entityType">The type of the entity that could not be found.</param>
        public EntityNotFoundException(string message, Exception inner, Type entityType)
            : this(message, inner)
        {
            EntityType = EnsureArg.IsNotNull(entityType, nameof(entityType));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        /// <summary>
        /// Type of the entity that could not be found.
        /// </summary>
        public Type EntityType { get; }
    }
}