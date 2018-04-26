// Copyright © 2018 Alex Leendertsen

namespace Editor.Models
{
    public class AppenderRefModel
    {
        public AppenderRefModel(string @ref)
        {
            Ref = @ref;
            IsEnabled = true;
        }

        public string Ref { get; }

        public bool IsEnabled { get; set; }
    }
}
