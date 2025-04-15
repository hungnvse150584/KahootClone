using BOs.Model;
using DAO.BaseDAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class ResponseDAO : BaseDAO<Response>
    {
        private readonly KahootDbContext _context;

        public ResponseDAO(KahootDbContext context) : base(context)
        {
            _context = context;
        }

        // Get Response by ID with related Player, Question, and Answer
        public async Task<Response> GetByIdAsync(int id)
        {
            var response = await _context.Responses
                .Include(r => r.Player)
                .Include(r => r.Question)
                .Include(r => r.Answer)
                .FirstOrDefaultAsync(r => r.ResponseId == id);

            if (response == null)
            {
                throw new ArgumentNullException($"Response with id {id} not found");
            }

            return response;
        }

        // Get Responses by PlayerId
        public async Task<List<Response>> GetResponsesByPlayerIdAsync(int playerId)
        {
            return await _context.Responses
                .Where(r => r.PlayerId == playerId)
                .Include(r => r.Player)
                .Include(r => r.Question)
                .Include(r => r.Answer)
                .ToListAsync();
        }

        // Get Responses by QuestionId
        public async Task<List<Response>> GetResponsesByQuestionIdAsync(int questionId)
        {
            return await _context.Responses
                .Where(r => r.QuestionId == questionId)
                .Include(r => r.Player)
                .Include(r => r.Question)
                .Include(r => r.Answer)
                .ToListAsync();
        }
    }
}
