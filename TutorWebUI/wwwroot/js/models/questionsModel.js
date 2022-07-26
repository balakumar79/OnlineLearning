class Model {
    constructor(QusId, QuestionName, StatusId,
        Topic = null, SubTopic = null, QuestionTypeId = null, TestId = null, SectionId = null, CorrectOption = [], Mark = 1, Options = []
    ) {
        this.QusId = QusId ?? 0;
        this.QuestionName = QuestionName;
        this.QuestionTypeId = QuestionTypeId ?? currentquestiontypeid;
        this.SectionId = SectionId ?? currenctsectionid;
        this.TestId = TestId ?? currenttestid;
        this.CorrectOption = JSON.stringify(CorrectOption);
        this.Mark = Mark;
        this.Topic = Topic;
        this.SubTopic = SubTopic;
        this.StatusId = StatusId;
        this.Options = Options;
    }
}