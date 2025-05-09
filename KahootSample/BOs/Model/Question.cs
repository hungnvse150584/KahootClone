﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOs.Model
{
    public class Question
    {
        [Key]
        public int? QuestionId { get; set; }
        public int? QuizId { get; set; }
        public string? Text { get; set; }
        public int? TimeLimit { get; set; } 
        public string? ImageUrl { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public string? CorrectOptions { get; set; }
        public int? OrderIndex { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string? Status { get; set; } // Thêm Status

        // Quan hệ
        [ForeignKey("QuizId")]
        public Quiz Quiz { get; set; }

        public ICollection<QuestionInGame> QuestionsInGame { get; set; } = new List<QuestionInGame>();
    }
}