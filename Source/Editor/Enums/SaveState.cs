// Copyright © 2019 Alex Leendertsen

namespace Editor.Enums
{
    public enum SaveState
    {
        /// <summary>
        /// State cannot be determined (probably because there's no existing file to compare against).
        /// </summary>
        Unknown,

        /// <summary>
        /// XML has changed.
        /// </summary>
        Changed,

        /// <summary>
        /// Saving XML to disk.
        /// </summary>
        Saving,

        /// <summary>
        /// All changes have been saved (XML in RAM matches XML on disk).
        /// </summary>
        Saved
    }
}
