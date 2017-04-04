﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GymDiaryCodeFirst.Models
{
    public class ExerciseStats
    {
        [Key]
        public int ExerciseStatsId { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        [Required]
        public int WorkoutId { get; set; }
        public Workout Workout { get; set; }
        public float? WeightInKg { get; set; }
        public string Reps { get; set; }
        public int Sets { get; set; }
    }
}