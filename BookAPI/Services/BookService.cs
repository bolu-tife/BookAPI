﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPI.Data;
using BookAPI.Entities;
using BookAPI.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Services
{
    public class BookService : IBook
    {
        private BookApiDataContext _context;
        public BookService(BookApiDataContext context)
        {
            _context = context;
        }


        public void Add(Book book)
        {
            _context.Add(book);
            _context.SaveChanges();
        }

        public async Task<bool> AddAsync(Book book)
        {
            try
            {
                await _context.AddAsync(book);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
       
        public async Task<bool> Delete(int Id)
        {
            var book = await _context.Books.FindAsync(Id);

            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Book>> GetAll() //GetAll
        {

            return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetById(int Id) //GetById
        {
            var book = await _context.Books.FindAsync(Id);

            return book;
        }

        public async Task<bool> Update(Book book) //Update
        {
            var bk = await _context.Books.FindAsync(book.Id);
            if (bk != null)
            {
                bk.Title = book.Title;
                bk.AuthorId = book.AuthorId;
                bk.GerneID = book.GerneID;
                bk.ISBN = book.ISBN;
                bk.YearPublish = book.YearPublish;
                bk.Rating = book.Rating;
                bk.Summary = book.Summary;

                await _context.SaveChangesAsync();
                return true;
            }

            return false;
          
    }
        
    }
}
