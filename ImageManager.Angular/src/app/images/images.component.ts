import { Component } from '@angular/core';
import swal from 'sweetalert2';

import { Image, ImageSnippet } from '@/_models';
import { AuthenticationService } from '@/_services';
import { ImagesService } from '@/_services/images.service';

@Component({ templateUrl: 'images.component.html' })
export class ImagesComponent {
    images: Image[];
    selectedFile: ImageSnippet;
    uploading: boolean;
    showMessage: boolean;

    constructor(private imagesServiec: ImagesService, private authService: AuthenticationService) { 
    }
    
    ngOnInit() {
        this.loadImages();
    }
    
    get isAuthenticated(){
        return this.authService.currentUserValue != null;
    }
    
    deleteImage(image: Image){
        swal({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            type: 'warning',
            showCancelButton: true,
            focusCancel: true,
            confirmButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
          }).then((result) => {
            if (result.value) {
                this.imagesServiec.delete(image).pipe().subscribe(deleted => {
                    if(deleted){
                        swal("Done", "Image has been deleted", "success");
                        this.loadImages();
                    }else{
                        swal("Error!", "image couldn't be deleted :(", "error");
                    }
                });
            }
          })
        
    }

    deleteAll(_swal: any){
        if(!this.isAuthenticated){
            _swal.show();
            return false;
        }
        this.imagesServiec.deleteAll().pipe().subscribe(deleted => {
            if(deleted){
                swal("Done", "All images has been cleared", "success");
                this.loadImages();
            }else{
                swal("Error!", "images couldn't be deleted :(", "error");
            }
        });
    }

    processImage(imageInput: any) {
        const file: File = imageInput.files[0];
        const reader = new FileReader();
        reader.addEventListener('load', (event: any) => {

            this.selectedFile = new ImageSnippet(event.target.result, file);

            this.selectedFile.pending = true;
            this.uploading = true;
            this.imagesServiec.upload(this.selectedFile.file).subscribe(
                (res) => {
                    this.onSuccess();
                },
                (err) => {
                    this.onError();
                })
        });

        reader.readAsDataURL(file);
    }

    private onSuccess() {
        this.selectedFile.pending = false;
        this.selectedFile.status = 'ok';
        this.displayMessage();
        this.loadImages();
    }

    private onError() {
        this.selectedFile.pending = false;
        this.selectedFile.status = 'fail';
        this.selectedFile.src = '';
        this.displayMessage();
    }

    private displayMessage(){
        this.showMessage = true;
        var self = this;
        setTimeout(function(){
            self.showMessage = false;
            self.uploading = false;
        }, 3000);
    }

    private loadImages(){
        this.imagesServiec.getAll().pipe().subscribe(images => {
            this.images = images;
        });
    }
}