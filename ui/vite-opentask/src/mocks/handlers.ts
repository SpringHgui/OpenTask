import {} from "@/apis";
import { faker } from "@faker-js/faker";
import { HttpResponse, http } from "msw";

export const handlers = [
  http.get("https://example.com/user", () => {
    return HttpResponse.json({
      id: "c7b3d8e0-5e0b-4b0f-8b3a-3b9f4b3d3b3d",
      firstName: faker.person.fullName(),
      lastName: "Maverick",
    });
  }),
];
