import { Component, OnInit, Input } from '@angular/core';
import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';
import { BingoCardModel } from 'src/api/api';
import { FormGroup, FormControl } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-download',
  templateUrl: './download.component.html',
  styleUrls: ['./download.component.scss']
})
export class DownloadComponent implements OnInit {

  @Input() bingoCards$: BehaviorSubject<BingoCardModel[]>;
  @Input() backgroundColor: string;
  @Input() borderColor: string;
  public downloadFormGroup: FormGroup;
  public loading: boolean = false;

  constructor() { }

  ngOnInit(): void {
    this.buildForm();
  }

  public downloadPDF(): void {
    this.loading = true;
    window.scrollTo(0,0);
    this.buildPDF();
  }

  public navigateBack(): void {
    this.bingoCards$.next([]);
  }

  private buildPDF(): void {
    let doc = new jsPDF('p', 'mm', this.downloadFormGroup.get('paperSize').value);
    const bingoCards = document.querySelectorAll('.bingocard');

    for (let i = 0; i < bingoCards.length; i++) {
      html2canvas(bingoCards[i] as HTMLElement).then(function (canvas) {
        let image = canvas.toDataURL('image/png');
        doc.addImage(image, 'PNG', 3, 10, 200, 200, '', <any>'MEDIUM');

        if (i + 1 === bingoCards.length) {
          doc.save('bingo-cards.pdf');
          this.loading = false;
        } else {
          doc.addPage();
        }
      }.bind(this));
    }
  }

  private buildForm(): void {
    this.downloadFormGroup = new FormGroup({
      paperSize: new FormControl('A4')
    });
  }
}
