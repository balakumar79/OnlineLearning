class QuestionModel {
    constructor(QusId, QuestionName, StatusId,
        Topic = null, SubTopic = null, QuestionTypeId = null, TestId = null, SectionId = null, CorrectOption = [], Mark = 1, Options = [], ComprehensionModels = null
    ) {
        this.QusId = QusId ?? 0;
        this.QuestionName = QuestionName;
        this.QuestionTypeId = QuestionTypeId ?? currentquestiontypeid;
        this.SectionId = SectionId;
        this.TestId = TestId ?? currenttestid;
        this.CorrectOption = JSON.stringify(CorrectOption);
        this.Mark = Mark;
        this.Topic = Topic;
        this.SubTopic = SubTopic;
        this.StatusId = StatusId;
        this.Options = Options;
        this.ComprehensionModels = ComprehensionModels;
    }
}
class ComperhesionQustion {
    constructor(TestId, QusId, CompQusId, SectionId) {
        this.TestId = TestId;
        this.QusId = QusId;
        this.CompQusId = CompQusId;
        this.SectionId = SectionId;
    }
}