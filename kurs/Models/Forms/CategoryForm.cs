﻿namespace kurs.Models.Forms
{
    public class CategoryForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set; }
    }
}