// Copyright © 2020 Alex Leendertsen

namespace Editor.Enums
{
    public enum ModelCreateResult
    {
        /// <summary>
        /// Model was created successfully.
        /// </summary>
        Success,

        /// <summary>
        /// Element was and unknown or unsupported appender.
        /// </summary>
        UnknownAppender,

        /// <summary>
        /// Element was unknown.
        /// </summary>
        UnknownElement
    }
}
