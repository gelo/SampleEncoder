import { ChangeDetectorRef, Component, OnDestroy } from '@angular/core';
import { ApiService } from '../api-service/api.service';
import { Subscription } from 'rxjs';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  imports:[ HttpClientModule, FormsModule, CommonModule],
  selector: 'app-encode-text',
  templateUrl: './encode-text.component.html',
  styleUrl: './encode-text.component.css',
  providers: [ ApiService ]
})

export class EncodeTextComponent implements OnDestroy {
  textToEncode: string = '';
  encodedText: string = '';
  encodingInProgress = false;
  private streamSubscription: Subscription = new Subscription();

  constructor(private apiService: ApiService, private cdr: ChangeDetectorRef) {}

  encodeText(): void {
    this.encodingInProgress = true;
  
    this.apiService.encodeText(this.textToEncode).subscribe(
      (response) => {
        this.encodedText = '';
        this.streamSubscription = this.apiService.getStream().subscribe(
          (item) => {
            if (item && item.Character) {
              this.encodedText = this.encodedText + item.Character;
              this.cdr.detectChanges();
            }
          },
          (error) => {
            console.error('Error in stream:', error);
            this.encodingInProgress = false;
          },
          () => {
            this.encodingInProgress = false;
          }
        );
      },
      (error) => {
        console.error('Error encoding text:', error);
        this.encodingInProgress = false;
      }
    );
  }

  cancelEncoding(): void {
    this.encodedText = '';
    if (this.streamSubscription) {
      this.streamSubscription.unsubscribe();
    }
    this.apiService.cancelEncoding().subscribe(
      (response) => {
        console.log('Encoding canceled:', response);
        this.encodingInProgress = false;
      },
      (error) => {
        this.encodingInProgress = false;
        console.error('Error canceling encoding:', error);
      }
    );
  }

  ngOnDestroy(): void {
    if (this.streamSubscription) {
      this.streamSubscription.unsubscribe();
    }
  }
}
