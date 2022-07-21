class Model {
    constructor(QusId: number, QuestionName: string, QuestionTypeId: number, TestId: number,
        SectionId: number, Mark: number, Options: JSON, CorrectOption: JSON, StatusId: number,
        Topic: string, SubTopic: string
    ) {
        QusId = QusId ?? 0;
        SectionId = SectionId ?? 0;
    }
     
}