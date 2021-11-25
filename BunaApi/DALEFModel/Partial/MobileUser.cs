﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DALEFModel
{
    /// <summary>
    /// Partial class for Activity model to add extra fields to model
    /// Author :Robin Cyrolies
    /// </summary>
    public partial class MobileUser
    {
        public string StpUserTypeDesc { get; set; }
        public string PlantDesc { get; set; }


    }
}