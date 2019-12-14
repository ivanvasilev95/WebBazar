import { Component, OnInit } from '@angular/core';
import { Ad } from 'src/app/_models/ad';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AdService } from 'src/app/_services/ad.service';
import { Category } from 'src/app/_models/category';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-ad-add',
  templateUrl: './ad-add.component.html',
  styleUrls: ['./ad-add.component.css']
})
export class NewAddComponent implements OnInit {
  ad: Ad;
  categories: Category[];
  createAdForm: FormGroup;

  constructor(private fb: FormBuilder, private router: Router, private alertify: AlertifyService,
              private adService: AdService, private authService: AuthService) { }

  ngOnInit() {
    this.getCategories();
    this.createNewAdForm();
  }

  getCategories() {
    this.adService.getCategories().subscribe((categories: Category[]) => this.categories = categories,
    error => this.alertify.error(error));
  }

  get categoryId() {
    return this.createAdForm.get('categoryId');
  }

  selectCategory(e) {
    if (typeof(e.target.value) === 'number') {
      this.categoryId.setValue(e.target.value, {
        onlySelf: true
      });
    } else {
      this.categoryId.setValue(+e.target.value.substring(0, 1), {
        onlySelf: true
      });
    }
  }

  createNewAdForm() {
    this.createAdForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
      categoryId: ['', Validators.required],
      description: ['', Validators.maxLength(20)],
      location: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
      price: ['', Validators.required],
      isUsed: [false]
    }, {validator: this.priceValidator});
  }

  priceValidator(g: FormGroup) {
    return g.get('price').value >= 0 ? null : {mismatch: true};
  }

  createAd() {
    if (this.createAdForm.valid) {
      this.ad = Object.assign({}, this.createAdForm.value);
      this.ad.userId = +this.authService.decodedToken.nameid;
      this.ad.categoryName = this.categories.find(c => c.id === this.ad.categoryId).name;
      console.log(this.ad);
      this.adService.createAd(this.ad).subscribe(() => {
        this.alertify.success('Ad successfully created.');
      }, error => {
        this.alertify.error(error);
      }, () => {
        this.router.navigate(['/user/ads']);
      });
    }
  }
}