using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpCheck_API.Constants;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.QuestionAndChoices;
using HelpCheck_API.Models;
using HelpCheck_API.Repositories.AmedAnswerDetails;
using HelpCheck_API.Repositories.AmedAnswerHeaders;
using HelpCheck_API.Repositories.QuestionAndChoices;
using HelpCheck_API.Repositories.Users;

namespace HelpCheck_API.Services.QuestionAndChoices
{
    public class QuestionAndChoiceService : IQuestionAndChoiceService
    {
        private readonly IQuestionAndChoiceRepository _questionAndChoiceRepository;
        private readonly IAmedAnswerDetailRepository _amedAnswerDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAmedAnswerHeaderRepository _amedAnswerHeaderRepository;

        public QuestionAndChoiceService(IQuestionAndChoiceRepository questionAndChoiceRepository, IAmedAnswerDetailRepository amedAnswerDetailRepository, IUserRepository userRepository, IAmedAnswerHeaderRepository amedAnswerHeaderRepository)
        {
            _questionAndChoiceRepository = questionAndChoiceRepository;
            _amedAnswerDetailRepository = amedAnswerDetailRepository;
            _userRepository = userRepository;
            _amedAnswerHeaderRepository = amedAnswerHeaderRepository;
        }

        public async Task<ResultResponse> GetAllQuestionAndChoiceAsync()
        {
            var data = await _questionAndChoiceRepository.GetAllQuestionMappingChoiceAsync();

            if (data is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = new List<GetQuestionAndChoiceDto>()
                };
            }

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = data
            };
        }

        public async Task<ResultResponse> GetQuestionAndChoiceByQuestionNumberAsync(string questionNum)
        {
            var data = await _questionAndChoiceRepository.GetQuestionAndChoiceByQuestionNumberAsync(questionNum);

            if (data is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = new List<GetQuestionAndChoiceDto>()
                };
            }

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = data
            };
        }

        public async Task<ResultResponse> CreateAnswerAsync(List<AddAnswerDto> addAnswerDtos, string accessToken)
        {
            try
            {
                var user = await _userRepository.GetUserByAccessTokenAsync(accessToken);
                string userFullName = user.TitleName + " " + user.FirstName + " " + user.LastName;

                if (addAnswerDtos != null && addAnswerDtos.Count > 0)
                {
                    foreach (var item in addAnswerDtos)
                    {
                        var answer = await _questionAndChoiceRepository.GetAnswerByQuestIDAsync(user.ID, item.QuestionID);
                        if (answer != null)
                        {
                            answer.ChoiceID = item.ChoiceID;
                            answer.ChoiceNum = item.ChoiceNum;
                            answer.AnswerKeyIn = item.AnswerKeyIn;
                            answer.UpdatedBy = userFullName;

                            var statusUpdated = await _amedAnswerDetailRepository.UpdateAmedAnswerDetailAsync(answer);

                            if (statusUpdated != Constant.STATUS_SUCCESS)
                            {
                                return new ResultResponse()
                                {
                                    IsSuccess = false,
                                    Data = statusUpdated
                                };
                            }
                        }
                        else
                        {
                            AmedAnswerDetail createModel = new AmedAnswerDetail()
                            {
                                QuestionID = item.QuestionID,
                                QuestionNum = item.QuestionNum,
                                ChoiceID = item.ChoiceID,
                                ChoiceNum = item.ChoiceNum,
                                AnswerKeyIn = item.AnswerKeyIn,
                                MemberID = user.ID,
                                CreatedBy = userFullName
                            };

                            string statusCreated = await _amedAnswerDetailRepository.CreateAmedAnswerDetailAsync(createModel);

                            if (statusCreated != Constant.STATUS_SUCCESS)
                            {
                                return new ResultResponse()
                                {
                                    IsSuccess = false,
                                    Data = statusCreated
                                };
                            }
                        }
                    }
                }

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = Constant.STATUS_SUCCESS
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = ex.Message
                };
            }
        }

        public async Task<ResultResponse> GetAnswerAsync(string accessToken, int year)
        {
            var user = await _userRepository.GetUserByAccessTokenAsync(accessToken);

            var answer = await _questionAndChoiceRepository.GetAnswerAsync(user.ID, year);

            if (answer is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = answer
            };
        }

        public async Task<ResultResponse> GetAnswerByMemberIDAsync(int memberId)
        {
            int yearNow = DateTime.Now.Year;
            var answer = await _questionAndChoiceRepository.GetAnswerAsync(memberId, yearNow);

            if (answer is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = answer
            };
        }

        public async Task<ResultResponse> GetAllAnswerAsync(int year)
        {
            try
            {
                var answer = await _questionAndChoiceRepository.GetAllAnswerAsync(year);

                if (answer is null)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Data = Constant.STATUS_DATA_NOT_FOUND
                    };
                }

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = answer
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = ex.Message
                };
            }
        }

        public async Task<ResultResponse> EditQuestionAndChoiceAsync(string accessToken, string questionNumber, List<EditChoiceDto> editChoiceDtos)
        {
            try
            {
                var user = await _userRepository.GetUserByAccessTokenAsync(accessToken);
                string userFullName = user.TitleName + " " + user.FirstName + " " + user.LastName;


                string statusRemoveChoice = await _questionAndChoiceRepository.RemoveChoiceInQuestionNumberAsync(questionNumber);
                if (statusRemoveChoice != Constant.STATUS_SUCCESS)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Data = statusRemoveChoice
                    };
                }

                foreach (var item in editChoiceDtos)
                {
                    AmedAnswerHeader data = new AmedAnswerHeader()
                    {
                        QuestionNum = questionNumber,
                        ChoiceNum = item.ChoiceNum
                    };

                    string status = await _amedAnswerHeaderRepository.CreateAmedAnswerHeaderAsync(data);
                    if (status != Constant.STATUS_SUCCESS)
                    {
                        return new ResultResponse()
                        {
                            IsSuccess = false,
                            Data = status
                        };
                    }
                }

                var response = await GetQuestionAndChoiceByQuestionNumberAsync(questionNumber);

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = response.Data
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = ex.Message
                };
            }
        }

        public async Task<ResultResponse> MappingChoiceWithQuestionAsync(string accessToken, string questionNumber, List<EditChoiceDto> editChoiceDtos)
        {
            try
            {
                var user = await _userRepository.GetUserByAccessTokenAsync(accessToken);
                string userFullName = user.TitleName + " " + user.FirstName + " " + user.LastName;

                await _questionAndChoiceRepository.RemoveChoiceInQuestionNumberAsync(questionNumber);
                
                foreach (var item in editChoiceDtos)
                {
                    AmedAnswerHeader data = new AmedAnswerHeader()
                    {
                        QuestionNum = questionNumber,
                        ChoiceNum = item.ChoiceNum
                    };

                    string status = await _amedAnswerHeaderRepository.CreateAmedAnswerHeaderAsync(data);
                    if (status != Constant.STATUS_SUCCESS)
                    {
                        return new ResultResponse()
                        {
                            IsSuccess = false,
                            Data = status
                        };
                    }
                }

                var response = await GetQuestionAndChoiceByQuestionNumberAsync(questionNumber);
                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = response.Data
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = ex.Message
                };
            }
        }
    }
}