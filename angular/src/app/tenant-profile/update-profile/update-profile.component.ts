import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse } from '@angular/common/http';
import { AppSessionService } from '../../../shared/session/app-session.service';
import { MatDialogRef } from '@angular/material';
import {
    PartnerServiceProxy,
    UpdateProfilePictureInput,
    UpdateProfilePictureOutput,
} from '@shared/service-proxies/service-proxies';

import { AppConsts } from '../../../shared/AppConsts';


declare var $: any;

@Component({
    selector: 'app-update-profile',
    templateUrl: './update-profile.component.html',
    styleUrls: [
        "./update-profile.component.css",
    ],
    styles: []

})
export class UpdateProfileComponent implements OnInit {

    
    uploadedFile: any = { fileName: null, height: 0, width: 0 };
    jcrop: any = null;

    @ViewChild("fileInput", { static: true }) fileInput;

    constructor(
        private http: HttpClient,
        private _partnerService: PartnerServiceProxy,
        private _sessionService: AppSessionService,
        private _dialogRef: MatDialogRef<UpdateProfileComponent>
    ) { }

    ngOnInit() {

    }

    addFile(): void {

        let fi = this.fileInput.nativeElement;
        if (fi.files && fi.files[0]) {
            let fileToUpload = fi.files[0];

            let input = new FormData();
            input.append("file", fileToUpload);
            this.http
                .post(AppConsts.remoteServiceBaseUrl+"/Tenant/UploadFile", input)
                .subscribe(res => {
                    //console.log(res['result'].fileName);
                    this.uploadedFile.fileName = res['result'].fileName;
                    this.uploadedFile.height = res['result'].height;
                    this.uploadedFile.width = res['result'].width;
                    setTimeout(() => { this.addJcrop() }, 10);
                    //this.addJcrop()
                    //this.updateProfilePicture(res['result'].fileName, res['result'].height, res['result'].width);
                });

        }
    }

    addJcrop(): void {

        var img = $("img#IconPictureResize");

        var ratio = this.uploadedFile.height * img.width() / this.uploadedFile.width;
        img.height(ratio + "px");

        var url = AppConsts.remoteServiceBaseUrl + "/Temp/Downloads/" + this.uploadedFile.fileName;

        if (this.jcrop) this.jcrop.data("Jcrop").destroy();
        img.attr("src", url);
        this.jcrop = img.Jcrop({
            trueSize: [this.uploadedFile.width, this.uploadedFile.height],
            setSelect: [0, 0, this.uploadedFile.width, this.uploadedFile.height]
        })
    }

    updateProfilePicture(): void {

        var n = { x: '0', y: '0', w: '0', h: '0' };
        if (this.jcrop) {
            n = this.jcrop.data("Jcrop").tellSelect();
        } else {
            n = { x: '0', y: '0', h: this.uploadedFile.height, w: this.uploadedFile.width };
        }
        let input = new UpdateProfilePictureInput();
        input.tenantId = this._sessionService.tenant.id;
        input.fileName = this.uploadedFile.fileName;
        input.x = parseInt(n.x, 10);
        input.y = parseInt(n.y, 10);
        input.height = parseInt(n.h, 10);
        input.width = parseInt(n.w, 10);

        this._partnerService
            .updateIconPicture(input)
            .subscribe((result: UpdateProfilePictureOutput) => {
                //console.dir(result);
                this.close(true);
            });
    }

    close(result: any): void {
        this._dialogRef.close(result);
    }
}
