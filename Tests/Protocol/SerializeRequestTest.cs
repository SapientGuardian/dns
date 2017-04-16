﻿using System.Collections.Generic;
using Xunit;
using DNS.Protocol;

namespace DNS.Tests.Protocol {
    
    public class SerializeRequestTest {
        [Fact]
        public void BasicQuestionRequestWithEmptyHeader() {
            Header header = new Header();

            Domain domain = new Domain(Helper.GetArray<string>());
            Question question = new Question(domain, RecordType.A, RecordClass.IN);
            IList<Question> questions = Helper.GetList(question);

            Request request = new Request(header, questions);

            byte[] content = Helper.ReadFixture("Request", "empty-header_basic-question");

            Assert.Equal(content, request.ToArray());
        }

        [Fact]
        public void SingleQuestionRequestWithHeader() {
            Header header = new Header();

            Domain domain = new Domain(Helper.GetArray("www", "google", "com"));
            Question question = new Question(domain, RecordType.CNAME, RecordClass.IN);
            IList<Question> questions = Helper.GetList(question);

            Request request = new Request(header, questions);
            request.Id = 1;
            request.RecursionDesired = true;

            byte[] content = Helper.ReadFixture("Request", "id-rd_www.google.com-cname");

            Assert.Equal(content, request.ToArray());
        }

        [Fact]
        public void RequestWithMultipleQuestions() {
            Header header = new Header();

            Domain domain1 = new Domain(Helper.GetArray("www", "google", "com"));
            Question question1 = new Question(domain1, RecordType.CNAME, RecordClass.IN);

            Domain domain2 = new Domain(Helper.GetArray("dr", "dk"));
            Question question2 = new Question(domain2, RecordType.A, RecordClass.ANY);

            Request request = new Request(header, new List<Question>());
            request.Id = 1;
            request.RecursionDesired = true;
            request.Questions.Add(question1);
            request.Questions.Add(question2);

            byte[] content = Helper.ReadFixture("Request", "multiple-questions");

            Assert.Equal(content, request.ToArray());
        }
    }
}
