using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Dtos
{
    public class GlobalCountViewDto
    {
        public int UserCount { get; set; }

        public int RoleCount { get; set; }

        public int ProductCount { get; set; }

        public int CategoryCount { get; set; }

        public int MessageCount { get; set; }
    }
}
