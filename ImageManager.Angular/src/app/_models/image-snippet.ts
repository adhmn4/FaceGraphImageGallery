export class ImageSnippet {
    constructor(public src: string, public file: File) { }

    pending: boolean = false;
    status: string = 'init';
}