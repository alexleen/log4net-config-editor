// Copyright © 2018 Alex Leendertsen

namespace Editor.Utilities
{
    public static class ExtensionsMethods
    {
        /// <summary>
        /// Evaluates to: HasValue && Value
        /// </summary>
        /// <param name="boolean"></param>
        /// <returns></returns>
        public static bool IsTrue(this bool? boolean)
        {
            return boolean.HasValue && boolean.Value;
        }
    }
}
