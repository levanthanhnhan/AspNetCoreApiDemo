import { Component } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.css']
})

export class LanguageComponent {

  constructor(private cookie: CookieService, private translate: TranslateService) {
    // Get lang
    const lang = this.cookie.get("lang") === "" ? "en" : this.cookie.get("lang");
    this.translate.setDefaultLang(lang);
    this.translate.use(lang);
  }

  switchLanguage(value) {
    this.cookie.set("lang", value);
    this.translate.use(value);
  }
}
