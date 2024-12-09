export class Trip {
    constructor(data) {
        this.id = data.id;
        this.name = data.name;
        this.description = data.description;
        this.url = data.url;
        this.tripStart = data.tripStart;
        this.tripEnd = data.tripEnd;
        this.creationDate = data.creationDate;
    }
}