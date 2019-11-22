import { Component, Injector, OnInit, Inject, Optional, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AppConsts } from '../../../../shared/AppConsts';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse } from '@angular/common/http'
import {
    
    VoucherServiceProxy,
    UpdateCoverPictureInput,
    UpdateCoverPictureOutput,
} from '@shared/service-proxies/service-proxies';
import { AppComponent } from '../../../app.component';


@Component({
  selector: 'app-update-banner',
  templateUrl: './update-banner.component.html',
  styleUrls: ['./update-banner.component.css']
})
export class UpdateBannerComponent extends AppComponent implements OnInit {

    uploadedFile: any = { fileName: null, height: 0, width: 0 };
    jcrop: any = null;

    @ViewChild("fileInput", { static: true }) fileInput;


    constructor(
        private http: HttpClient,
        injector: Injector,
        private _voucherService: VoucherServiceProxy,
        private _dialogRef: MatDialogRef<UpdateBannerComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _id: number
    ) {
        super(injector);
    }

    ngOnInit() {

    }

    addFile(): void {

        let fi = this.fileInput.nativeElement;
        if (fi.files && fi.files[0]) {
            let fileToUpload = fi.files[0];

            let input = new FormData();
            input.append("file", fileToUpload);
            this.http
                .post(AppConsts.remoteServiceBaseUrl + "/Voucher/UploadVoucherPlatformCoverPicture", input)
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
        this.jcrop = (<any>img).Jcrop({
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
        let input = new UpdateCoverPictureInput();
        input.targetId = this._id;
        input.fileName = this.uploadedFile.fileName;
        input.x = parseInt(n.x, 10);
        input.y = parseInt(n.y, 10);
        input.height = parseInt(n.h, 10);
        input.width = parseInt(n.w, 10);

        this._voucherService
            .updateVoucherPlatformCoverPicture(input)
            .subscribe((result: UpdateCoverPictureOutput) => {
                //console.dir(result);
                this.close(true);
            });
    }


    close(result: any): void {
        this._dialogRef.close(result);
    }

}
