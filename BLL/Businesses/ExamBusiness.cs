using BLL.Businesses.Base;
using Common.Extensions;
using Common.Helpers;
using DAL.Entities;
using DAL.Repositories.Base;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Businesses
{
    public class ExamBusiness : BaseBusiness<ExamEntity>
    {
        private readonly IRepository<QuestionEntity> _questionRepository;
        private readonly IRepository<AnswerEntity> _answerRepository;
        public ExamBusiness(IRepository<ExamEntity> repository, IRepository<QuestionEntity> questionRepository, IRepository<AnswerEntity> answerRepository) : base(repository)
        {
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
        }

        public override async Task<ExamEntity> AddAsync(ExamEntity entity)
        {
            var insertedExam = await this._repository.InsertAsync(entity).ConfigureAwait(false);
            entity.Questions.ForEach(async (question) =>
            {
                question.ExamId = insertedExam.Id;
                var insertedQuestion = await this._questionRepository.InsertAsync(question).ConfigureAwait(false);
                question.Id = insertedQuestion.Id;
                question.Question = insertedQuestion.Question;
                question.Answers.ForEach(async (answer) =>
                {
                    answer.QuestionId = insertedQuestion.Id;
                    var insertedAnswer = await this._answerRepository.InsertAsync(answer).ConfigureAwait(false);
                    answer.Id = insertedAnswer.Id;
                    answer.Answer = insertedAnswer.Answer;
                    answer.Correctness = insertedAnswer.Correctness;
                });
            });
            return insertedExam;
        }

        public override async Task<ExamEntity> UpdateAsync(ExamEntity entity)
        {
            var updatedExam = await this._repository.UpdateAsync(entity, new string[] { "Title", "Content" }).ConfigureAwait(false);
            entity.Questions.ForEach(async (question) =>
            {
                var updatedQuestion = await this._questionRepository.UpdateAsync(question).ConfigureAwait(false);
                question.Answers.ForEach(async (answer) =>
                {
                    var updatedAnswer = await this._answerRepository.UpdateAsync(answer).ConfigureAwait(false);
                });
            });
            return updatedExam;
        }

        public override async Task<ExamEntity> GetAsync(long id)
        {
            var exam = await this._repository.GetAsync(id).ConfigureAwait(false);
            if (exam != null)
            {
                exam.Questions = (await this._questionRepository.GetAsync("ExamId", id).ConfigureAwait(false))?.ToList();
                exam.Questions?.ForEach(async (y) =>
                {
                    y.Answers = (await this._answerRepository.GetAsync("QuestionId", y.Id).ConfigureAwait(false))?.ToList();
                });
            }
            return exam;
        }

        public async Task<IEnumerable<ExamEntity>> GetUserAttendedExamsAsync(long userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@userId", value: userId, dbType: DbType.Int64, direction: ParameterDirection.Input);
            return await this._repository.QueryAsync($@"
                SELECT Exams.* FROM Exams 
                INNER JOIN UserAttendedExams ON UserAttendedExams.UserId = {userId} AND UserAttendedExams.ExamId = Exams.Id;", 
                parameters).ConfigureAwait(false);
        }

        public async Task<ExamEntity> GetUserCleanExamAsync(long examId)
        {
            var exam = await this._repository.GetAsync(examId).ConfigureAwait(false);
            if (exam != null)
            {
                exam.Questions = (await this._questionRepository.GetAsync("ExamId", examId).ConfigureAwait(false))?.ToList();
                exam.Questions?.ForEach(async (x) =>
                {
                    x.Answers = (await this._answerRepository.GetAsync("QuestionId", x.Id).ConfigureAwait(false))?.ToList();
                    x.Answers?.ForEach(y => y.Correctness = null);
                });
            }
            return exam;
        }
    }
}