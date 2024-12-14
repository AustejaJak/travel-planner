export class Activity {
    constructor(data) {
        this.id = data.id;
        this.name = data.name;
        this.type = data.type;
        this.startTime = data.startTime;
        this.endTime = data.endTime;
        this.creationDate = data.creationDate;
    }
}