import { Injectable } from "@angular/core";
import { environment } from "../environtments/environtment";
import { HttpClient } from "@angular/common/http";

@Injectable({
    providedIn: 'root'
})

export class AuthService {
      private apiUrl = environment.apiURL + "auth";

      constructor(private http: HttpClient) {}

    login(username: string, password: string) {
        return this.http.post<any>(`${this.apiUrl}/login`, { username, password });
    }
}